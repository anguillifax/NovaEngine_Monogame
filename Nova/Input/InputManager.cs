using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public static class InputManager {

		public static event Action InputUpdate;

		public static readonly DebugButton Quit = new DebugButton(Keys.F4);
		public static readonly DebugButton TestLoadBindings = new DebugButton(Keys.F1);
		public static readonly DebugButton TestLoadBindingsDef = new DebugButton(Keys.F3);
		public static readonly DebugButton TestSaveBindings = new DebugButton(Keys.F2);
					   
		public static readonly DebugButton RebindingPanel = new DebugButton(Keys.F10);
		public static readonly DebugButton InputsPanel = new DebugButton(Keys.F11);

		public static readonly InputSourceKeyboard SourceKeyboard = new InputSourceKeyboard();
		public static readonly InputSourceGamepad SourceGamepad1 = new InputSourceGamepad(PlayerIndex.One);
		public static readonly InputSourceGamepad SourceGamepad2 = new InputSourceGamepad(PlayerIndex.Two);

		public static readonly CompoundInputSource Player1 = new CompoundInputSource(SourceKeyboard, SourceGamepad1);
		public static readonly CompoundInputSource Player2 = new CompoundInputSource(SourceGamepad1, SourceGamepad2);
		public static readonly CompoundInputSource Any = new CompoundInputSource(SourceKeyboard, SourceGamepad1, SourceGamepad2);

		public static void Update() {
			InputUpdate?.Invoke();
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