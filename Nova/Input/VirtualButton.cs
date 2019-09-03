using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Project.Input {

	public class VirtualButton {
		public readonly VirtualKeyboardButton keyboard;
		public readonly VirtualKeyboardButton keyboardDefault;
		public readonly VirtualGamepadButton gamepad;
		public readonly VirtualGamepadButton gamepadDefault;

		private bool value, previousValue;

		public VirtualButton() {
			InputManager.InputUpdate += Update;
		}

		private void Update() {
			keyboard.Update();
			gamepad.Update();

			previousValue = value;
			value = keyboard.Value || gamepad.Value;
		}

		#region Accessors

		/// <summary>
		/// Returns true if button is pressed
		/// </summary>
		public bool Pressed {
			get { return value; }
		}

		/// <summary>
		/// Returns true if button was pressed this frame
		/// </summary>
		public bool JustPressed {
			get { return value && value != previousValue; }
		}

		/// <summary>
		/// Returns true if button not being pressed
		/// </summary>
		public bool Released {
			get { return !value; }
		}

		/// <summary>
		/// Returns true if button was released this frame
		/// </summary>
		public bool JustReleased {
			get { return !value && value != previousValue; }
		}

		#endregion
	}

	public class VirtualKeyboardButton {

		public Keys Key { get; private set; }

		public bool Value { get; private set; }

		public VirtualKeyboardButton() : this(Keys.None) {
		}

		public VirtualKeyboardButton(Keys key) {
			Key = key;
		}

		public void Update() {
			Value = Keyboard.GetState().IsKeyDown(Key);
		}

		/// <summary>
		/// Sets the new key if key is allowed. Returns true if successful.
		/// </summary>
		public bool SetKey(Keys newKey) {
			if (!InputProperties.BlacklistedKeys.Contains(newKey)) {
				Key = newKey;
				return true;
			}
			return false;
		}

	}

	public class VirtualGamepadButton {

		public static int MaxButtons = 4;
		public List<Buttons> buttons;

		public bool Value { get; private set; }

		public VirtualGamepadButton() {
			buttons = new List<Buttons>(MaxButtons);
		}

		public VirtualGamepadButton(params Buttons[] btns) {
			buttons = new List<Buttons>(btns);
		}

		public void Update() {
			Value = false;
			foreach (var button in buttons) {
				if (GamePad.GetState(PlayerIndex.One).IsButtonDown(button)) Value = true;
			}
		}

		public bool SetButton(Buttons newButton) {
			if (buttons.Contains(newButton)) {
				buttons.Remove(newButton);
				return true;
			}

			if (InputProperties.WhitelistedButtons.Contains(newButton)) {
				buttons.Add(newButton);
				return true;
			}

			return false;
		}

	}

}