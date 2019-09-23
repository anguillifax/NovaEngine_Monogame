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

		Actor actor;
		Action hitSomething = () => Console.WriteLine("Hit something!");

		public void Init() {


		}

		void Test(float x, float y) {
			actor.Move(new Vector2(x, y), hitSomething);
			Console.WriteLine(actor.Position);
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