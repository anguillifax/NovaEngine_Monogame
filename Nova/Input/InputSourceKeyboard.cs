using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Nova.Input {

	public class InputSourceKeyboard : InputSource {

		private KeyboardBindingData SaveData {
			get { return BindingManager.CurrentBindings.Keyboard; }
		}

		public readonly List<VirtualKeyboardButton> AllButtons;

		public InputSourceKeyboard() {

			AllButtons = new List<VirtualKeyboardButton>();

			CreateButton(ref Enter, new VirtualKeyboardButton(BindingNames.Enter, Keys.Enter));
			CreateButton(ref Back, new VirtualKeyboardButton(BindingNames.Back, Keys.Escape));
			CreateButton(ref Clear, new VirtualKeyboardButton(BindingNames.Clear, Keys.Back));
			
			CreateButton(ref Jump, new VirtualKeyboardButton(BindingNames.Jump));
		}

		private void CreateButton(ref VirtualButton vb, VirtualKeyboardButton vkb) {
			AllButtons.Add(vkb);
			vb = vkb;
		}

	}

}