using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public class InputSourceGamepad : InputSource {

		public PlayerIndex Index { get; set; }
		public bool RumbleEnabled { get; set; }

		public readonly List<VirtualGamepadButton> AllButtons;

		public InputSourceGamepad(PlayerIndex playerIndex) {
			Index = playerIndex;

			Engine.Instance.Exiting += (s, e) => StopRumbling();

			AllButtons = new List<VirtualGamepadButton>();

			CreateButton(ref Enter, new VirtualGamepadButton(BindingNames.Enter, Index, Buttons.A));
			CreateButton(ref Back, new VirtualGamepadButton(BindingNames.Back, Index, Buttons.Start));
			CreateButton(ref Clear, new VirtualGamepadButton(BindingNames.Clear, Index, Buttons.Back));

			CreateButton(ref Jump, new VirtualGamepadButton(BindingNames.Jump, Index));
		}

		private void CreateButton(ref VirtualButton vb, VirtualGamepadButton vgb) {
			AllButtons.Add(vgb);
			vb = vgb;
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