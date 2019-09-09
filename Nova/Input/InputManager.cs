using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public static class MInput {

		public static event Action InputUpdate;
		public static event Action InputUpdateLate;

		public static readonly SimpleButton Quit = new SimpleButton(Keys.F4);
		public static readonly SimpleButton TestLoadBindings = new SimpleButton(Keys.F1);
		public static readonly SimpleButton TestLoadBindingsDef = new SimpleButton(Keys.F3);
		public static readonly SimpleButton TestSaveBindings = new SimpleButton(Keys.F2);

		public static readonly SimpleButton Cycle = new SimpleButton(Keys.F8);
		public static readonly SimpleButton TestToggle = new SimpleButton(Keys.F7);

		public static readonly SimpleButton RebindingPanelKeyboard = new SimpleButton(Keys.F9);
		public static readonly SimpleButton RebindingPanelGamepad1 = new SimpleButton(Keys.F10);
		public static readonly SimpleButton RebindingPanelGamepad2 = new SimpleButton(Keys.F11);
		public static readonly SimpleButton InputsPanel = new SimpleButton(Keys.F12);

		public static readonly InputSourceKeyboard SourceKeyboard = new InputSourceKeyboard();
		public static readonly InputSourceGamepad SourceGamepad1 = new InputSourceGamepad(PlayerIndex.One);
		public static readonly InputSourceGamepad SourceGamepad2 = new InputSourceGamepad(PlayerIndex.Two);

		public static readonly CompoundInputSource Player1 = new CompoundInputSource(SourceKeyboard);
		public static readonly CompoundInputSource Player2 = new CompoundInputSource(SourceKeyboard, SourceGamepad1);
		public static readonly CompoundInputSource Any = new CompoundInputSource(SourceKeyboard, SourceGamepad1, SourceGamepad2);

		public static void Update() {
			InputUpdate?.Invoke();
			InputUpdateLate?.Invoke();

			if (TestToggle.Pressed) {
				Player1.SetRumble(1f);
			} else {
				Player1.StopRumbling();
			}
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