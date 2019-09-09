using Microsoft.Xna.Framework.Input;

namespace Nova.Input {

	public class VirtualKeyboardButton : VirtualButtonBaseLogic {

		public Keys? UserKey;
		public readonly Keys? HardcodedKey;

		public VirtualKeyboardButton(string name) :
			base(name) {
			HardcodedKey = null;
		}

		public VirtualKeyboardButton(string name, Keys? hardcodedKey) :
			base (name) {
			HardcodedKey = hardcodedKey;
		}

		protected override void Update() {
			lastValue = value;

			var s = Keyboard.GetState();

			bool user = false;
			if (UserKey != null) {
				user = s.IsKeyDown((Keys)UserKey);
			}

			bool hardcoded = false;
			if (HardcodedKey != null) {
				hardcoded = s.IsKeyDown((Keys)HardcodedKey);
			}

			value = user || hardcoded;

		}

		protected override void OnLoadBinding() {
			UserKey = BindingManager.CurrentBindings.Keyboard[Name];
		}

		protected override void OnSaveBinding() {
			BindingManager.CurrentBindings.Keyboard[Name] = UserKey;
		}

		public RebindResult Rebind(Keys newKey) {

			if (GlobalInputProperties.IsKeyAllowed(newKey)) {

				if (newKey == HardcodedKey) {
					UserKey = null;
					return RebindResult.Removed;

				} else if (newKey == UserKey) {
					UserKey = null;
					return RebindResult.Removed;

				} else {
					UserKey = newKey;
					return RebindResult.Added;
				}

			} else {
				return RebindResult.NoOp;
			}
		}

		public void Unbind() {
			UserKey = null;
		}

	}

}