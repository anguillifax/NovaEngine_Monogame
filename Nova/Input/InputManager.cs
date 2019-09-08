using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public static class InputManager {

		public static event Action InputUpdate;

		public static DebugButton Quit = new DebugButton(Keys.F4);
		public static DebugButton TestLoadBindings = new DebugButton(Keys.F1);
		public static DebugButton TestLoadBindingsDef = new DebugButton(Keys.F3);
		public static DebugButton TestSaveBindings = new DebugButton(Keys.F2);

		public static DebugButton RebindingPanel = new DebugButton(Keys.F10);

		public static readonly InputSource SourceKeyboard = new InputSourceKeyboard();
		public static readonly InputSource SourceGamepad1 = new InputSourceGamepad(PlayerIndex.One);
		public static readonly InputSource SourceGamepad2 = new InputSourceGamepad(PlayerIndex.Two);

		public static readonly CompoundInputSource Player1 = new CompoundInputSource();
		public static readonly CompoundInputSource Player2 = new CompoundInputSource();
		public static readonly CompoundInputSource Any = new CompoundInputSource(SourceKeyboard, SourceGamepad1, SourceGamepad2);

		public static void Update() {
			InputUpdate?.Invoke();

			if (Any.Enter.JustPressed) {
				Console.WriteLine("enter");
			}
		}

		public static void LoadBindings() {
			BindingsManager.Load();
		}

		public static void LoadDefaultBindings() {
			BindingsManager.LoadDefault();
		}

		public static void SaveBindings() {
			BindingsManager.Save();
		}

	}

}