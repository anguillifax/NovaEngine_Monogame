using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public class Engine : Game {

		public static Engine Instance { get; private set; }

		public static Scene CurrentScene { get; private set; }
		public static bool IsPaused { get; set; }

		public static SpriteFont DefaultFont { get; private set; }

		GraphicsDeviceManager graphics;

		public QuickTest quickTest;

		public Engine() {
			Instance = this;

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.ApplyChanges();

			Window.AllowUserResizing = true;
			Window.Title = "Nova Engine";

			Content.RootDirectory = "Content";

			IsPaused = false;

			quickTest = new QuickTest();

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			MDraw.Initialize(GraphicsDevice);
			Screen.Update(GraphicsDevice.Viewport.Bounds);

			IsMouseVisible = true;

			CurrentScene = new Scene();

			InputManager.LoadBindings();
			Console.WriteLine("Gamepad 1 connected: {0}", GamePad.GetState(PlayerIndex.One).IsConnected);
			Console.WriteLine("Gamepad 2 connected: {0}", GamePad.GetState(PlayerIndex.Two).IsConnected);

			Components.Add(new Gui.PanelRebindKeyboard(this));
			Components.Add(new Gui.PanelRebindGamepad(this, PlayerIndex.One));
			Components.Add(new Gui.PanelRebindGamepad(this, PlayerIndex.Two));

			quickTest.Init();

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {

			var tex = Content.Load<Texture2D>("Images/bullet");
			//CurrentScene.Add(new TestEntity(tex));

			DefaultFont = Content.Load<SpriteFont>("Font1");

			Camera.Position = Screen.Center;
			Camera.Scale = 1;

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
			Screen.Update(graphics.GraphicsDevice.Viewport.Bounds);
			InputManager.Update();
			Gui.DebugGUI_Inputs.Update();

			if (InputManager.Quit.JustPressed) {
				Exit();
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

			base.Update(time);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="time">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime time) {
			Time.UpdateDraw(time);
			GraphicsDevice.Clear(Color.Black);

			MDraw.Begin();
			if (CurrentScene != null) {
				CurrentScene.Draw();
			}
			MDraw.End();

			Gui.DebugGUI_Inputs.Draw();

			quickTest.Draw();

			base.Draw(time);
		}

	}

}
