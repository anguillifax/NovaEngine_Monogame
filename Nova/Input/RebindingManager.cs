using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public static class RebindingManager {

		private readonly static List<Keys> lastKeys = new List<Keys>();
		private readonly static List<Buttons> lastButtons = new List<Buttons>();

		public static VirtualButton Target {
			get {
				return target;
			}
			set {
				if (value != target) {
					target = value;
					lastKeys.Clear();
					lastButtons.Clear();
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

			for (int i = 0; i < lastKeys.Count; i++) {
				if (s.IsKeyUp(lastKeys[i])) {
					lastKeys.RemoveAt(i);
					i--;
				}
			}

			Keys[] currentKeys = Keyboard.GetState().GetPressedKeys();

			foreach (var key in currentKeys) {
				if (!lastKeys.Contains(key) && IsAllowedInMenu(key)) {
					var result = Target.RebindKeyboard(key);
					lastKeys.Add(key);
					Console.WriteLine("{2} {0} key {1}", Target.Name, key, result.ToString());
				}
			}

		}

		private static bool IsAllowedInMenu(Keys key) {
			return !(key == Keys.Left || key == Keys.Right || key == Keys.Up || key == Keys.Down);
		}

		private static void SearchGamepad() {

			GamePadState s = GamePad.GetState(PlayerIndex.One);

			for (int i = 0; i < lastButtons.Count; i++) {
				if (s.IsButtonUp(lastButtons[i])) {
					lastButtons.RemoveAt(i);
					i--;
				}
			}

			foreach (var button in InputProperties.WhitelistedButtons) {
				if (s.IsButtonDown(button) && !lastButtons.Contains(button)) {
					var result = Target.RebindGamepad(button);
					lastButtons.Add(button);
					Console.WriteLine("{2} {0} button {1}", Target.Name, button, result.ToString());
				}
			}

		}


	}

}