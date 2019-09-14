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
				MDraw.Camera.Zoom += 0.05f * (InputManager.Any.Horizontal.Value + InputManager.Any.Vertical.Value);
			}
			if (ButtonF6.Pressed) {
				MDraw.Camera.WorldPosition.X += 5f * InputManager.Any.Horizontal.Value;
				MDraw.Camera.WorldPosition.Y += 5f * InputManager.Any.Vertical.Value;
			}
		}

		public void Draw() {

		}

	}

}