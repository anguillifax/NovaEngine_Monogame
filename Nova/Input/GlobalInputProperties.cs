using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Nova.Input {

	public static class BindingNames {
		public static readonly string Enter = "enter";
		public static readonly string Back = "back";
		public static readonly string Clear = "clear";

		public static readonly string Horz = "horz";
		public static readonly string Vert = "vert";

		public static readonly string Jump = "jump";
	}

	public static class GlobalInputProperties {

		public static readonly int MaxGamepadButtons = 4;

		public static float AxisDeadzone = 0.2f;
		public static float AxisLowPowerThreshold = 0.85f;
		public static float AxisLowPowerAmount = 0.6f;

		public static float DelayUntilRepeat = 0.4f;
		public static float RepeatDelay = 0.1f;

		/// <summary>
		/// Keyboard keys that are already reserved for other functions
		/// </summary>
		private static readonly Keys[] BlacklistedKeys = new Keys[] {
			Keys.Escape, Keys.Enter, Keys.Left, Keys.Right, Keys.Up, Keys.Down,
			Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
		};

		public static bool IsKeyAllowed(Keys key) {
			return !BlacklistedKeys.Contains(key);
		}

		/// <summary>
		/// Gamepad buttons that allow remapping
		/// </summary>
		public static readonly Buttons[] WhitelistedButtons = new Buttons[] {
			Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
			Buttons.LeftShoulder, Buttons.RightShoulder,
			Buttons.LeftTrigger, Buttons.RightTrigger,
			Buttons.LeftStick, Buttons.RightStick
		};

		public static bool IsButtonAllowed(Buttons button) {
			return WhitelistedButtons.Contains(button);
		}

		public static float CleanAxisInput(float v) {
			float sign = Math.Sign(v);
			v = Math.Abs(v);

			if (v < AxisDeadzone) {
				v = 0f;
			} else if (v < AxisLowPowerThreshold) {
				v = sign * AxisLowPowerAmount;
			} else {
				v = 1f * sign;
			}
			return v;
		}
	}

}