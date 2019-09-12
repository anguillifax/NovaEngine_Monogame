using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public class QuickTest {

		SimpleButton ButtonF5 = new SimpleButton(Keys.F5);
		SimpleButton ButtonF6 = new SimpleButton(Keys.F6);
		SimpleButton ButtonF7 = new SimpleButton(Keys.F7);

		public void Init() {

		}

		public void Update() {
			if (ButtonF5.Pressed) {
				InputManager.Player1.SetRumble(0.5f);
			} else {
				InputManager.Player1.StopRumbling();
			}
		}

		public void Draw() {

		}

	}

}