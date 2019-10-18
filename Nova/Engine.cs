using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nova.PhysicsEngine;
using Nova.Tiles;
using System;
using System.Collections.Generic;

namespace Nova {

	public class Engine : Game {

		public static Engine Instance { get; private set; }

		public Scene CurrentScene { get; private set; }
		public bool IsPaused { get; set; }

		public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

		public QuickTest quickTest;

		public const int TileSize = 7;
		public const float FrameRate = 30;
		public static readonly Point ScreenSizeInTiles = new Point(36, 20);
		public static readonly Point ScreenSizeInPixels = new Point(TileSize * ScreenSizeInTiles.X, TileSize * ScreenSizeInTiles.Y);

		public Engine() {
			Instance = this;

			GraphicsDeviceManager = new GraphicsDeviceManager(this);
			GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
			GraphicsDeviceManager.PreferredBackBufferHeight = 720;
			GraphicsDeviceManager.SynchronizeWithVerticalRetrace = true;
			GraphicsDeviceManager.ApplyChanges();

			Window.ClientSizeChanged += (s, e) => UpdateViewport();

			Window.AllowUserResizing = true;
			Window.Title = "Nova Engine";
			IsMouseVisible = true;

			TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FrameRate);

			Content.RootDirectory = "Content";

			IsPaused = false;

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {

			lastWindowDimensions = GetDimensions(Window.ClientBounds);
			Viewport = GraphicsDevice.Viewport;

			Screen.Update();
			MDraw.Initialize();
			InputManager.Init();

			CurrentScene = new Scene("MainScene");

			Components.Add(new Gui.PanelRebindKeyboard(this));
			Components.Add(new Gui.PanelRebindGamepad(this, PlayerIndex.One));
			Components.Add(new Gui.PanelRebindGamepad(this, PlayerIndex.Two));
			Components.Add(new Gui.PanelRebindInputSources(this));

			quickTest = new QuickTest();
			quickTest.Init();

			base.Initialize();

		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {

			MDraw.LoadContent();

			quickTest.LoadContent();

			var grass = Content.Load<Texture2D>("Images/Tiles/grass");
			var dirt = Content.Load<Texture2D>("Images/Tiles/dirt");
			var deepDirt = Content.Load<Texture2D>("Images/Tiles/deep_dirt");
			var temple = Content.Load<Texture2D>("Images/Tiles/temple");
			var player = Content.Load<Texture2D>("Images/Tiles/player");


			/*
			new DebugTestPhysics(CurrentScene);
			//*/


			//*
			 
			new TestActor(CurrentScene, new Vector2(2, 1), temple);
			new TestSolid(CurrentScene, grass, new Vector2(2, 0), new Vector2(1, 1));
			new TestSolid(CurrentScene, grass, new Vector2(3, -1), new Vector2(1, 1));
			new TestSolid(CurrentScene, grass, new Vector2(3, -2), new Vector2(1, 1));
			//new TestSolid(CurrentScene, grass, new Vector2(5, 0), new Vector2(1, 1));

			//*/

			CurrentScene.Init();
			Time.Init();

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime time) {

			Time.Update(time);
			InputManager.Update();
			Gui.DebugGUI_Inputs.Update();
			DebugCameraController.Update();

			if (InputManager.Quit.JustPressed) {
				Exit();
			}

			if (InputManager.Any.Enter.JustPressed) {
				IsPaused = !IsPaused;
			}

			if (InputManager.ClearConsole.JustPressed) {
				Console.Clear();
			}

			if (InputManager.TestLoadBindings.JustPressed) InputManager.LoadBindings();
			if (InputManager.TestLoadBindingsDef.JustPressed) InputManager.LoadDefaultBindings();
			if (InputManager.TestSaveBindings.JustPressed) InputManager.SaveBindings();

			quickTest.Update();

			if (IsPaused) {
				base.Update(time);
				return;
			}

			if (CurrentScene != null) {
				CurrentScene.PreUpdate();
				CurrentScene.Update();
				CurrentScene.PostUpdate();
			}

			Physics.Update();

			base.Update(time);
		}

		double lastTimeOfDraw;

		protected override void Draw(GameTime time) {

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Viewport = Viewport;
			GraphicsDevice.Clear(new Color(5, 5, 5));

			Screen.Update();
			Time.UpdateDraw(time);

			if (CurrentScene != null) {
				MDraw.Begin();
				CurrentScene.Draw();
				MDraw.End();
			}

			Physics.Draw();

			Gui.DebugGUI_Inputs.Draw();

			quickTest.Draw();

			//DrawWindowDebugPoints()
			//DrawFPS();

			base.Draw(time);
			lastTimeOfDraw = Time.ExactTimeSinceStartup;
		}

		void DrawFPS() {
			double calcDeltaTime = Time.ExactTimeOfDraw - lastTimeOfDraw;
			MDraw.SpriteBatch.Begin();
			MDraw.SpriteBatch.DrawString(MDraw.DefaultFont, $"FPS: {1f / calcDeltaTime:f2}", new Vector2(Screen.Width - 80, 10),
				new Color(1, 1, 1, 0.4f), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
			MDraw.End();
		}

		void DrawWindowDebugPoints() {
			MDraw.Begin();
			MDraw.DrawPoint(Vector2.Zero, Color.White); // World Origin
			MDraw.DrawPointGlobal(Screen.Center, Color.White); // Screen Center
			MDraw.DrawBoxGlobal(Screen.Center, Screen.Center - new Vector2(0.5f), new Color(50, 50, 50)); // Screen extents
			MDraw.End();
		}


		private Point lastWindowDimensions;

		private Point GetDimensions(Rectangle r) {
			return new Point(r.Width, r.Height);
		}

		public static float WidescreenRatio { get { return 16f / 9; } }
		public static Viewport Viewport { get; private set; }

		private void UpdateViewport() {

			var dimensions = GetDimensions(Window.ClientBounds);
			if (lastWindowDimensions == dimensions) return;
			lastWindowDimensions = dimensions;

			float ratio = (float)dimensions.X / dimensions.Y;

			if (ratio > WidescreenRatio) {
				var optimalDimensions = new Point((int)(dimensions.Y * WidescreenRatio), dimensions.Y);
				int leftEdge = (dimensions.X - optimalDimensions.X) / 2;
				Viewport = new Viewport(leftEdge, 0, optimalDimensions.X, optimalDimensions.Y, 0, 1);

			} else if (ratio < WidescreenRatio) {
				var optimalDimensions = new Point(dimensions.X, (int)(dimensions.X / WidescreenRatio));
				int topEdge = (dimensions.Y - optimalDimensions.Y) / 2;
				Viewport = new Viewport(0, topEdge, optimalDimensions.X, optimalDimensions.Y, 0, 1);

			} else {
				Viewport = new Viewport(0, 0, dimensions.X, dimensions.Y, 0, 1);
			}

			MDraw.Camera.CalculateScale();
		}

	}

}
