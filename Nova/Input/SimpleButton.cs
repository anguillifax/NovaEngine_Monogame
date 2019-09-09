using Microsoft.Xna.Framework.Input;

namespace Nova.Input {

	/// <summary>
	/// A simplified button that only supports keyboard presses.
	/// </summary>
	public class SimpleButton {

		private readonly Keys key;
		private bool value, previousValue;

		public SimpleButton(Keys key) {
			this.key = key;
			MInput.InputUpdate += Update;
		}

		void Update() {
			previousValue = value;
			value = Keyboard.GetState().IsKeyDown(key);
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