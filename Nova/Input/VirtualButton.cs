using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public enum RebindResult {
		Added, Removed, NoOp
	}

	public class VirtualButton {

		public readonly string Name;
		public readonly bool GamepadRebindable;

		public bool IsVirtualAxisMovementKey;

		public readonly VirtualKeyboardButton keyboard;
		public readonly VirtualKeyboardButton keyboardDefault;
		public readonly VirtualGamepadButton gamepad;

		private bool value, previousValue;

		/// <summary>
		/// Create a new VirtualButton and set hardcoded inputs.
		/// </summary>
		public VirtualButton(string name, Keys kb, params Buttons[] gp) {
			Name = name;
			keyboard = new VirtualKeyboardButton();
			keyboardDefault = new VirtualKeyboardButton(kb);
			gamepad = new VirtualGamepadButton(gp);
			GamepadRebindable = false;

			InputManager.InputUpdate += Update;
			InputBindingsManager.LoadBindings += OnLoadBindings;
			InputBindingsManager.SaveBindings += OnSaveBindings;

			InputManager.AllButtons.Add(this);
		}

		/// <summary>
		/// Create a virtual button with no hardcoded inputs.
		/// </summary>
		public VirtualButton(string name) {
			Name = name;
			keyboard = new VirtualKeyboardButton();
			keyboardDefault = new VirtualKeyboardButton();
			gamepad = new VirtualGamepadButton();
			GamepadRebindable = true;

			InputManager.InputUpdate += Update;
			InputBindingsManager.LoadBindings += OnLoadBindings;
			InputBindingsManager.SaveBindings += OnSaveBindings;


			InputManager.AllButtons.Add(this);
		}

		protected void Update() {
			keyboard.Update();
			keyboardDefault.Update();
			gamepad.Update();

			previousValue = value;
			value = keyboard.Value || keyboardDefault.Value || gamepad.Value;
		}

		protected void OnSaveBindings() {
			BindingData data;
			if (GamepadRebindable) {
				data = new BindingData(keyboard.Key, gamepad.buttons);
			} else {
				data = new BindingData(keyboard.Key);
			}
			InputBindingsManager.CurrentBindings[Name] = data;
		}

		protected void OnLoadBindings() {
			var data = InputBindingsManager.CurrentBindings[Name];
			if (data == null) { // There is no saved data. Unbind rebindable inputs.
				keyboard.Unbind();
				if (GamepadRebindable) {
					gamepad.UnbindAll();
				}
			} else { // Found data. Attempt to rebind current inputs.
				keyboard.Set(data.Keyboard);
				if (GamepadRebindable) {
					gamepad.Set(data.Gamepad);
				}
			}
		}

		public RebindResult RebindKeyboard(Keys newKey) {
			if (newKey == keyboardDefault.Key) {
				if (keyboard.HasKey) {
					keyboard.Unbind();
					return RebindResult.Removed;
				} else {
					return RebindResult.NoOp;
				}
			} else {
				return keyboard.Rebind(newKey);
			}
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

		public Keys? Key { get; private set; }

		public bool Value { get; private set; }

		public bool HasKey {
			get { return Key != null; }
		}

		public VirtualKeyboardButton() : this(null) {
		}

		public VirtualKeyboardButton(Keys? key) {
			Key = key;
		}

		public void Update() {
			if (Key == null) {
				Value = false;
			} else {
				Value = Keyboard.GetState().IsKeyDown((Keys)Key);
			}
		}

		/// <summary>
		/// Sets the new key if key is allowed. Returns true if successful.
		/// </summary>
		public RebindResult Rebind(Keys? newKey) {
			if (newKey == null) return RebindResult.NoOp;

			if (Key == newKey) {
				Key = null;
				return RebindResult.Removed;
			}

			if (!InputProperties.BlacklistedKeys.Contains((Keys)newKey)) {
				Key = newKey;
				return RebindResult.Added;
			}

			return RebindResult.NoOp;
		}

		/// <summary>
		/// Overrides binding without checking blacklist.
		/// </summary>
		/// <param name="newKey"></param>
		public void Set(Keys? newKey) {
			Key = newKey;
		}

		public void Unbind() {
			Key = null;
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

		/// <summary>
		/// Attempts to add a new button.
		/// </summary>
		public RebindResult Rebind(Buttons newButton) {
			if (buttons.Contains(newButton)) {
				buttons.Remove(newButton);
				return RebindResult.Removed;
			}

			if (InputProperties.WhitelistedButtons.Contains(newButton)) {
				buttons.Add(newButton);
				if (buttons.Count > InputProperties.MaxGamepadButtons) {
					buttons.RemoveAt(0);
				}
				return RebindResult.Added;
			}

			return RebindResult.NoOp;
		}

		/// <summary>
		/// Overrides any preexisting keys without checking whitelist.
		/// </summary>
		public void Set(List<Buttons> newButtons) {
			buttons.Clear();
			buttons.AddRange(newButtons);
			if (buttons.Count > InputProperties.MaxGamepadButtons) {
				buttons.RemoveRange(InputProperties.MaxGamepadButtons, buttons.Count - InputProperties.MaxGamepadButtons);
			}
		}

		public void UnbindAll() {
			buttons.Clear();
		}

	}

}