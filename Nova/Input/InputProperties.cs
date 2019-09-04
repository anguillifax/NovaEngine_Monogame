using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public static class InputProperties {

		public static readonly int MaxGamepadButtons = 4;

		/// <summary>
		/// Keyboard keys that are already reserved for other functions
		/// </summary>
		public static readonly Keys[] BlacklistedKeys = new Keys[] {
			Keys.Escape, Keys.Enter, Keys.Left, Keys.Right, Keys.Up, Keys.Down,
			Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
			Keys.CapsLock, Keys.NumLock
		};

		/// <summary>
		/// Gamepad buttons that allow remapping
		/// </summary>
		public static readonly Buttons[] WhitelistedButtons = new Buttons[] {
			Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
			Buttons.LeftShoulder, Buttons.RightShoulder,
			Buttons.LeftTrigger, Buttons.RightTrigger,
			Buttons.LeftStick, Buttons.RightStick
		};

	}

}