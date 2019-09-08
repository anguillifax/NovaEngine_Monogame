using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public class InputSourceKeyboard : InputSource {

		public InputSourceKeyboard() {
			Enter = new VirtualKeyboardButton("enter", Keys.Enter);
			Back = new VirtualKeyboardButton("back", Keys.Escape);
			Clear = new VirtualKeyboardButton("clear", Keys.Back);

			Jump = new VirtualKeyboardButton("jump");
		}

		public override void LoadBindings() {
			throw new NotImplementedException();
		}

		public override void SaveBindings() {
			throw new NotImplementedException();
		}

	}

}