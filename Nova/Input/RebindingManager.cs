using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Project.Input {

	public static class RebindingManager {

		private static Keys? lastKey = null;
		private static Buttons? lastButton = null;

		public static VirtualButton Target {
			get {
				return target;
			}
			set {
				if (value != target) {
					target = value;
					lastKey = null;
					lastButton = null;
				}
			}
		}
		private static VirtualButton target;

		public static void Update() {

			if (Target == null) return;

			SearchKeyboard();

			if (Target.GamepadRebindable) {
				SearchGamepad();
			}

		}

		private static void SearchKeyboard() {

			var s = Keyboard.GetState();

			if (lastKey != null) {
				if (s.IsKeyUp((Keys)lastKey)) {
					lastKey = null;
					Console.WriteLine("Keyboard Up");
				}
			}

			Keys[] currentKeys = Keyboard.GetState().GetPressedKeys();

			foreach (var key in currentKeys) {
				if (!InputProperties.BlacklistedKeys.Contains(key) && key != lastKey) {
					var result = Target.RebindKeyboard(key);
					lastKey = key;
					Console.WriteLine("{2} {0} key {1}", Target.Name, key, result.ToString());
				}
			}

		}

		private static void SearchGamepad() {

			GamePadState s = GamePad.GetState(PlayerIndex.One);

			if (lastButton != null) {
				if (s.IsButtonUp((Buttons)lastButton)) {
					lastButton = null;
					Console.WriteLine("Gamepad Up");
				}
			}

			foreach (var button in InputProperties.WhitelistedButtons) {
				if (s.IsButtonDown(button) && button != lastButton) {
					var result = Target.RebindGamepad(button);
					lastButton = button;
					Console.WriteLine("{2} {0} button {1}", Target.Name, button, result.ToString());
				}
			}

		}

	}

}