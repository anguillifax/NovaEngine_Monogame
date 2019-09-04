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

		public readonly VirtualKeyboardButton SourceKeyboard;
		public readonly VirtualKeyboardButton SourceKeyboardDefault;
		public readonly VirtualGamepadButton SourceGamepad;

		private bool value, previousValue;

		public readonly InputRepeater Repeater;

		private VirtualButton() {
			SourceKeyboard = new VirtualKeyboardButton();
			Repeater = new InputRepeater(() => Pressed);

			InputManager.InputUpdate += Update;
			InputBindingsManager.LoadBindings += OnLoadBindings;
			InputBindingsManager.SaveBindings += OnSaveBindings;

			InputManager.AllButtons.Add(this);
		}

		/// <summary>
		/// Create a new VirtualButton and set hardcoded inputs.
		/// </summary>
		public VirtualButton(string name, Keys kb, params Buttons[] gp) : this() {
			Name = name;
			SourceKeyboardDefault = new VirtualKeyboardButton(kb);
			SourceGamepad = new VirtualGamepadButton(gp);
			GamepadRebindable = false;

		}

		/// <summary>
		/// Create a virtual button with no hardcoded inputs.
		/// </summary>
		public VirtualButton(string name) : this() {
			Name = name;
			SourceKeyboardDefault = new VirtualKeyboardButton();
			SourceGamepad = new VirtualGamepadButton();
			GamepadRebindable = true;
		}

		protected void Update() {
			SourceKeyboard.Update();
			SourceKeyboardDefault.Update();
			SourceGamepad.Update();

			previousValue = value;
			value = SourceKeyboard.Pressed || SourceKeyboardDefault.Pressed || SourceGamepad.Pressed;

			Repeater.Update();
		}

		protected void OnSaveBindings() {
			BindingData data;
			if (GamepadRebindable) {
				data = new BindingData(SourceKeyboard.Key, SourceGamepad.buttons);
			} else {
				data = new BindingData(SourceKeyboard.Key);
			}
			InputBindingsManager.CurrentBindings[Name] = data;
		}

		protected void OnLoadBindings() {
			var data = InputBindingsManager.CurrentBindings[Name];
			if (data == null) { // There is no saved data. Unbind rebindable inputs.
				SourceKeyboard.Unbind();
				if (GamepadRebindable) {
					SourceGamepad.UnbindAll();
				}
			} else { // Found data. Attempt to rebind current inputs.
				SourceKeyboard.Set(data.Keyboard);
				if (GamepadRebindable) {
					SourceGamepad.Set(data.Gamepad);
				}
			}
		}

		public RebindResult RebindKeyboard(Keys newKey) {
			if (newKey == SourceKeyboardDefault.Key) {
				if (SourceKeyboard.HasKey) {
					SourceKeyboard.Unbind();
					return RebindResult.Removed;
				} else {
					return RebindResult.NoOp;
				}
			} else {
				return SourceKeyboard.Rebind(newKey);
			}
		}

		public RebindResult RebindGamepad(Buttons newButton) {
			if (GamepadRebindable) {
				return SourceGamepad.Rebind(newButton);
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

		#region Special Accessors

		/// <summary>
		/// Returns true if buttons was pushed this frame. Only checks hardcoded inputs.
		/// </summary>
		public bool JustPressedHardcoded {
			get {
				if (GamepadRebindable) {
					return SourceKeyboardDefault.JustPressed;
				} else {
					return SourceKeyboardDefault.JustPressed || SourceGamepad.JustPressed;
				}
			}
		}

		#endregion

	}

	public class VirtualKeyboardButton {

		public Keys? Key { get; private set; }

		private bool value, previousValue;

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
				previousValue = false;
				value = false;
			} else {
				previousValue = value;
				value = Keyboard.GetState().IsKeyDown((Keys)Key);
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

	public class VirtualGamepadButton {

		public List<Buttons> buttons;

		private bool value, previousValue;

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
			previousValue = value;
			value = false;
			foreach (var button in buttons) {
				if (GamePad.GetState(PlayerIndex.One).IsButtonDown(button)) value = true;
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

}