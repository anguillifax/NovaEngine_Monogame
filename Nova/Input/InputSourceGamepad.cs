using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public class InputSourceGamepad : InputSource {

		public PlayerIndex Index { get; set; }
		public bool RumbleEnabled { get; set; }

		public InputSourceGamepad(PlayerIndex playerIndex) {
			Index = playerIndex;

			Engine.Instance.Exiting += (s, e) => StopRumbling();

			Enter = new VirtualGamepadButton("enter", Index, Buttons.A);
			Back = new VirtualGamepadButton("back", Index, Buttons.Start);
			Clear = new VirtualGamepadButton("clear", Index, Buttons.Back);

			Jump = new VirtualGamepadButton("jump", Index);
		}

		public override void LoadBindings() {
			throw new NotImplementedException();
		}

		public override void SaveBindings() {
			throw new NotImplementedException();
		}

		public void SetRumble(float left, float right) {
			GamePad.SetVibration(Index, left, right);
		}

		public void SetRumble(float power) {
			SetRumble(power, power);
		}

		public void StopRumbling() {
			SetRumble(0);
		}

	}

}