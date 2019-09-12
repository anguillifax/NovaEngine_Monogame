using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public static class InputManager {

		public static event Action InputUpdate;

		public static readonly SimpleButton Quit = new SimpleButton(Keys.F4);
		public static readonly SimpleButton TestLoadBindings = new SimpleButton(Keys.F1);
		public static readonly SimpleButton TestLoadBindingsDef = new SimpleButton(Keys.F3);
		public static readonly SimpleButton TestSaveBindings = new SimpleButton(Keys.F2);

		public static readonly SimpleButton Cycle = new SimpleButton(Keys.F8);
		public static readonly SimpleButton TestToggle = new SimpleButton(Keys.F7);

		public static readonly SimpleButton RebindingPanelInputSources = new SimpleButton(Keys.F8);
		public static readonly SimpleButton RebindingPanelKeyboard = new SimpleButton(Keys.F9);
		public static readonly SimpleButton RebindingPanelGamepad1 = new SimpleButton(Keys.F10);
		public static readonly SimpleButton RebindingPanelGamepad2 = new SimpleButton(Keys.F11);
		public static readonly SimpleButton InputsPanel = new SimpleButton(Keys.F12);

		public static readonly InputSourceKeyboard SourceKeyboard = new InputSourceKeyboard();
		public static readonly InputSourceGamepad SourceGamepad1 = new InputSourceGamepad(PlayerIndex.One);
		public static readonly InputSourceGamepad SourceGamepad2 = new InputSourceGamepad(PlayerIndex.Two);

		public static readonly CompoundInputSource Player1 = new CompoundInputSource();
		public static readonly CompoundInputSource Player2 = new CompoundInputSource();
		public static readonly CompoundInputSource Any = new CompoundInputSource(SourceKeyboard, SourceGamepad1, SourceGamepad2);

		public static void Update() {
			InputUpdate?.Invoke();
		}

		public static void Init() {
			InputSourceLayoutManager.Init();
			LoadBindings();

			//Console.WriteLine("Gamepad 1 connected: {0}", GamePad.GetState(PlayerIndex.One).IsConnected);
			//Console.WriteLine("Gamepad 2 connected: {0}", GamePad.GetState(PlayerIndex.Two).IsConnected);
		}

		public static void LoadBindings() {
			BindingManager.Load();
		}

		public static void LoadDefaultBindings() {
			BindingManager.LoadDefault();
		}

		public static void SaveBindings() {
			BindingManager.Save();
		}

	}

}