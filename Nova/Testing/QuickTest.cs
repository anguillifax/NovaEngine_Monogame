using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		Texture2D bg;

		public void Init() {
			Vector2 v = new Vector2(1f, 0f);
			for (int i = 0; i < 8; i++) {
				//Console.WriteLine("{0} deg: {1} => {2}", i * 45, v, Calc.RotateDegrees(v, i * 45));
			}
		}

		public void LoadContent() {

		}

		public void Update() {
			if (ButtonF5.Pressed) {
				smoothZoom += 0.01f * (InputManager.Any.Horizontal.Value + InputManager.Any.Vertical.Value);
				smoothZoom = Calc.Clamp(smoothZoom, -3f, 3f);
				MDraw.Camera.Zoom = (float)Math.Pow(10, smoothZoom);
			}
			if (ButtonF6.Pressed) {
				MDraw.Camera.WorldPosition.X += 0.3f * InputManager.Any.Horizontal.Value;
				MDraw.Camera.WorldPosition.Y += 0.3f * InputManager.Any.Vertical.Value;
			}
			if (InputManager.Any.Jump.JustPressed) {
				//MDraw.Camera.Zoom = Calc.Round(MDraw.Camera.Zoom, 1);
			}
		}


		public void Draw() {

		}

	}

}