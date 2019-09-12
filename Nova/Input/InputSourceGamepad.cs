using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input.GamepadAxisDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public class InputSourceGamepad : InputSource {

		public PlayerIndex Index { get; set; }

		private bool internalRumbleEnabled;
		public bool RumbleEnabled {
			get {
				return internalRumbleEnabled;
			}
			set {
				internalRumbleEnabled = value;
				if (!internalRumbleEnabled) StopRumbling();
			}
		}

		public readonly List<VirtualGamepadButton> AllButtons;

		public VirtualGamepadButton this[string name] {
			get { return AllButtons.First(x => x.Name == name); }
		}

		private GamepadBindingData CurrentBindingData {
			get {
				return BindingManager.CurrentBindings.GetGamepad(Index);
			}
		}

		public InputSourceGamepad(PlayerIndex playerIndex) {
			Index = playerIndex;

			RumbleEnabled = true;

			AllButtons = new List<VirtualGamepadButton>();

			CreateButton(ref Enter, new VirtualGamepadButton(BindingNames.Enter, Index, Buttons.A));
			CreateButton(ref Back, new VirtualGamepadButton(BindingNames.Back, Index, Buttons.Start));
			CreateButton(ref Clear, new VirtualGamepadButton(BindingNames.Clear, Index, Buttons.Back));

			Horizontal = new VirtualGamepadAxis(BindingNames.Horz, Index,
				new StickLeftHorz(), new DPadHorz());
			Vertical = new VirtualGamepadAxis(BindingNames.Vert, Index,
				new StickLeftVert(), new DPadVert());

			CreateButton(ref Jump, new VirtualGamepadButton(BindingNames.Jump, Index));
			CreateButton(ref Attack, new VirtualGamepadButton(BindingNames.Attack, Index));
			CreateButton(ref Unleash, new VirtualGamepadButton(BindingNames.Unleash, Index));
			CreateButton(ref Retry, new VirtualGamepadButton(BindingNames.Retry, Index));

			BindingManager.LoadBindings += OnLoadBindings;
			BindingManager.SaveBindings += OnSaveBindings;
		}

		private void CreateButton(ref VirtualButton vb, VirtualGamepadButton vgb) {
			AllButtons.Add(vgb);
			vb = vgb;
		}

		private void OnLoadBindings() {
			RumbleEnabled = CurrentBindingData.RumbleEnabled;
		}

		private void OnSaveBindings() {
			CurrentBindingData.RumbleEnabled = RumbleEnabled;
		}

		public void SetRumble(float left, float right) {
			if (RumbleEnabled) {
				GamePad.SetVibration(Index, left, right);
			}
		}

		public void SetRumble(float power) {
			SetRumble(power, power);
		}

		public void StopRumbling() {
			GamePad.SetVibration(Index, 0, 0);
		}

	}

}