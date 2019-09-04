using Microsoft.Xna.Framework.Input;
using Nova.Input;

namespace Nova {

	/// <summary>
	/// A simplified button that only accepts keyboard presses.
	/// </summary>
	public class DebugButton : VirtualInput {

		VirtualKeyboardButton btn;
		private bool value, previousValue;

		public DebugButton(Keys key) :
			base(false) {
			btn = new VirtualKeyboardButton(key);
		}

		protected override void Update() {
			btn.Update();
			previousValue = value;
			value = btn.Value;
		}

		protected override void OnLoadBindings() {
		}

		protected override void OnSaveBindings() {
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