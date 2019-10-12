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

		public static readonly int TileSize = 7;
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

			//int offset = 2;
			//for (int i = 0; i < 5; i++) {
			//	CurrentScene.Add(new Tile(CurrentScene, grass, new IntVector2(i - offset, -4)));
			//	CurrentScene.Add(new Tile(CurrentScene, dirt, new IntVector2(i - offset, -5)));
			//	CurrentScene.Add(new Tile(CurrentScene, deepDirt, new IntVector2(i - offset, -6)));
			//}

			//CurrentScene.Add(new TestEntity(CurrentScene, player));

			new TestEntity(CurrentScene, Vector2.Zero, temple);
			new SolidFloor(CurrentScene, grass, new Vector2(2, 0), new Vector2(1, 1));

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="time">Provides a snapshot of timing values.</param>
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

			if (IsPaused) {
				base.Update(time);
				return;
			}

			if (InputManager.TestLoadBindings.JustPressed) {
				InputManager.LoadBindings();
			}
			if (InputManager.TestLoadBindingsDef.JustPressed) {
				InputManager.LoadDefaultBindings();
			}
			if (InputManager.TestSaveBindings.JustPressed) {
				InputManager.SaveBindings();
			}

			//Console.WriteLine("{0} x {1}", GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight);


			quickTest.Update();

			//if (InputManager.Any.Back.JustPressed) {
			//	IsPaused = !IsPaused;
			//	Console.WriteLine(IsPaused ? "Paused" : "Unpaused");
			//}

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

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="time">Provides a snapshot of timing values.</param>
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

			base.Draw(time);
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
