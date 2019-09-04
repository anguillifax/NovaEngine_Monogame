using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public enum RebindResult {
		Added, Removed, NoOp
	}

	public class VirtualButton : VirtualInput {

		public readonly string Name;

		private readonly VirtualKeyboardButton keyboard;
		private readonly VirtualKeyboardButton keyboardDefault;
		private readonly VirtualGamepadButton gamepad;

		private bool value, previousValue;

		/// <summary>
		/// Create a new VirtualButton and set hardcoded inputs.
		/// </summary>
		public VirtualButton(string name, Keys kb, params Buttons[] gp) :
			base(false) {
			Name = name;
			keyboard = new VirtualKeyboardButton();
			keyboardDefault = new VirtualKeyboardButton(kb);
			gamepad = new VirtualGamepadButton(gp);
		}

		/// <summary>
		/// Create a virtual button with no hardcoded inputs.
		/// </summary>
		public VirtualButton(string name) :
			base(true) {
			Name = name;
			keyboard = new VirtualKeyboardButton();
			keyboardDefault = new VirtualKeyboardButton();
			gamepad = new VirtualGamepadButton();
		}

		protected override void Update() {
			keyboard.Update();
			keyboardDefault.Update();
			gamepad.Update();

			previousValue = value;
			value = keyboard.Value || keyboardDefault.Value || gamepad.Value;
		}

		protected override void OnSaveBindings() {
			BindingData data;
			if (GamepadRebindable) {
				data = new BindingData(keyboard.Key, gamepad.buttons);
			} else {
				data = new BindingData(keyboard.Key);
			}
			InputBindingsManager.CurrentBindings[Name] = data;
		}

		protected override void OnLoadBindings() {
			var data = InputBindingsManager.CurrentBindings[Name];
			if (data == null) { // There is no saved data. Unbind rebindable inputs.
				keyboard.Unbind();
				if (GamepadRebindable) {
					gamepad.UnbindAll();
				}
			} else { // Found data. Attempt to rebind current inputs.
				keyboard.Rebind(data.Keyboard);
				if (GamepadRebindable) {
					gamepad.RebindAll(data.Gamepad);
				}
			}
		}

		public RebindResult RebindKeyboard(Keys newKey) {
			return keyboard.Rebind(newKey);
		}

		public RebindResult RebindGamepad(Buttons newButton) {
			if (GamepadRebindable) {
				return gamepad.Rebind(newButton);
			}
			return RebindResult.NoOp;
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

		public bool HasKey {
			get { return Key != Keys.None; }
		}

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
		public RebindResult Rebind(Keys newKey) {
			if (Key == newKey) {
				Key = Keys.None;
				return RebindResult.Removed;
			}
			if (!InputProperties.BlacklistedKeys.Contains(newKey)) {
				Key = newKey;
				return RebindResult.Added;
			}
			return RebindResult.NoOp;
		}

		public void Unbind() {
			Key = Keys.None;
		}

	}

	public class VirtualGamepadButton {

		public List<Buttons> buttons;

		public bool Value { get; private set; }

		public bool HasButton {
			get { return buttons.Count > 0; }
		}

		public VirtualGamepadButton() {
			buttons = new List<Buttons>(InputProperties.MaxGamepadButtons);
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

		public RebindResult Rebind(Buttons newButton) {
			if (buttons.Contains(newButton)) {
				buttons.Remove(newButton);
				return RebindResult.Removed;
			}

			if (InputProperties.WhitelistedButtons.Contains(newButton)) {
				buttons.Add(newButton);
				return RebindResult.Added;
			}

			return RebindResult.NoOp;
		}

		public void RebindAll(List<Buttons> newButtons) {
			buttons.Clear();
			buttons.AddRange(newButtons);
		}

		public void UnbindAll() {
			buttons.Clear();
		}

	}

}