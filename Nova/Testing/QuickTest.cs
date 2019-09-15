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

		float smoothZoom = 0;

		public void Init() {

		}

		public void Update() {
			if (ButtonF5.Pressed) {
				smoothZoom += 0.05f * (InputManager.Any.Horizontal.Value + InputManager.Any.Vertical.Value);
				smoothZoom = Calc.Clamp(smoothZoom, -3f, 3f);
				Console.WriteLine("{0} => {1}", smoothZoom, Math.Exp(smoothZoom));
				MDraw.Camera.Zoom = (float)Math.Exp(smoothZoom);
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