using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public static class GamepadRebindingManager {

		private readonly static List<Buttons> lastButtons = new List<Buttons>();

		public static VirtualGamepadButton Target { get; private set; }

		public static PlayerIndex Index { get; private set; }

		private static readonly List<Buttons> prevButtons = new List<Buttons>();

		public static void Update() {
			if (Target == null) return;

			var s = GamePad.GetState(Index);

			for (int i = 0; i < lastButtons.Count; i++) {
				if (s.IsButtonUp(lastButtons[i])) {
					lastButtons.RemoveAt(i);
					i--;
				}
			}

			foreach (var key in GlobalInputProperties.WhitelistedButtons) {
				if (!lastButtons.Contains(key)) {
					var ret = Target.Rebind(key);
					lastButtons.Add(key);
				}
			}

		}

		public static void BeginRebinding(VirtualGamepadButton target) {
			Target = target;
			lastButtons.Clear();
			foreach (var b in GlobalInputProperties.WhitelistedButtons) {
				lastButtons.Add(b);
			}
			prevButtons.ClearAdd(Target.ButtonList);
		}

		public static void Unbind() {
			Target?.Unbind();
		}

		public static void CancelOperation() {
			Target.ButtonList.ClearAdd(lastButtons);
			//Target = null;
		}

	}

}