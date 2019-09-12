using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public class InputSourceKeyboard : InputSource {

		public readonly List<VirtualKeyboardButton> AllButtons;

		public VirtualKeyboardButton this[string name] {
			get { return AllButtons.First(x => x.Name == name); }
		}

		public InputSourceKeyboard() {

			AllButtons = new List<VirtualKeyboardButton>();

			CreateButton(ref Enter, new VirtualKeyboardButton(BindingNames.Enter, Keys.Enter));
			CreateButton(ref Back, new VirtualKeyboardButton(BindingNames.Back, Keys.Escape));
			CreateButton(ref Clear, new VirtualKeyboardButton(BindingNames.Clear, Keys.Back));

			CreateAxis(ref Horizontal, new VirtualKeyboardAxis(BindingNames.Horz, Keys.Right, Keys.Left));
			CreateAxis(ref Vertical, new VirtualKeyboardAxis(BindingNames.Vert, Keys.Up, Keys.Down));

			CreateButton(ref Jump, new VirtualKeyboardButton(BindingNames.Jump));
			CreateButton(ref Attack, new VirtualKeyboardButton(BindingNames.Attack));
			CreateButton(ref Unleash, new VirtualKeyboardButton(BindingNames.Unleash));
			CreateButton(ref Retry, new VirtualKeyboardButton(BindingNames.Retry));

		}

		private void CreateButton(ref VirtualButton vb, VirtualKeyboardButton vkb) {
			AllButtons.Add(vkb);
			vb = vkb;
		}

		private void CreateAxis(ref VirtualAxis v, VirtualKeyboardAxis axis) {
			AllButtons.Add(axis.Pos);
			AllButtons.Add(axis.Neg);
			v = axis;
		}

	}

}