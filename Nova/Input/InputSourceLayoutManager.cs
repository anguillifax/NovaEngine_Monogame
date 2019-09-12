namespace Nova.Input {

	public static class InputSourceLayoutManager {

		internal static void Init() {
			BindingManager.LoadBindings += ApplyFromCurrent;
			BindingManager.SaveBindings += SaveFromCurrent;
		}

		public static void ApplyFromCurrent() {
			ApplyConfiguration(BindingManager.CurrentBindings.InputSourceLayout);
		}

		public static void SaveFromCurrent() {
			BindingManager.CurrentBindings.InputSourceLayout.keyboardIsPlayer2 = InputManager.Player2.Sources.Contains(InputManager.SourceKeyboard);
			BindingManager.CurrentBindings.InputSourceLayout.gamepad1IsPlayer2 = InputManager.Player2.Sources.Contains(InputManager.SourceGamepad1);
		}

		private static void ApplyConfiguration(InputSourceLayoutData toggles) {

			if (toggles.keyboardIsPlayer2) { // KB is Player 2
				if (toggles.gamepad1IsPlayer2) { // GP1 is Player 2
					InputManager.Player1.SetNewSources(InputManager.SourceGamepad2);
					InputManager.Player2.SetNewSources(InputManager.SourceKeyboard, InputManager.SourceGamepad1);
				} else { // GP1 is Player 1
					InputManager.Player1.SetNewSources(InputManager.SourceGamepad1);
					InputManager.Player2.SetNewSources(InputManager.SourceKeyboard, InputManager.SourceGamepad2);
				}
			} else { // KB is Player 1
				if (toggles.gamepad1IsPlayer2) { // GP1 is Player 2
					InputManager.Player1.SetNewSources(InputManager.SourceKeyboard, InputManager.SourceGamepad2);
					InputManager.Player2.SetNewSources(InputManager.SourceGamepad1);
				} else { // GP1 is Player 1
					InputManager.Player1.SetNewSources(InputManager.SourceKeyboard, InputManager.SourceGamepad1);
					InputManager.Player2.SetNewSources(InputManager.SourceGamepad2);
				}
			}

			InputManager.Player1.RetrieveNewVirtualInputs();
			InputManager.Player2.RetrieveNewVirtualInputs();

		}

	}

}