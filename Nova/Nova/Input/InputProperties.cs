using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project.Input {

	public static class InputProperties {

		public static readonly Keys[] BlacklistedKeys = new Keys[] {
			Keys.Escape, Keys.Enter, Keys.Left, Keys.Right, Keys.Up, Keys.Down
		};

		public static readonly Buttons[] WhitelistedButtons = new Buttons[] {
			Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
			Buttons.LeftShoulder, Buttons.RightShoulder,
			Buttons.LeftTrigger, Buttons.RightTrigger,
			Buttons.LeftStick, Buttons.RightStick
		};

	}

}