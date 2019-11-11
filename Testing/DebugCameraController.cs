using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;

namespace Nova {

	/// <summary>
	/// Provides basic operations to zoom and pan camera.
	/// </summary>
	public static class DebugCameraController {

		static readonly SimpleButton ButtonZoom = new SimpleButton(Keys.F5);
		static readonly SimpleButton ButtonPan = new SimpleButton(Keys.F6);
		static readonly SimpleButton ButtonReset = new SimpleButton(Keys.F7);

		static float smoothZoom = 0;

		public static void Update() {
			if (ButtonZoom.Pressed) {
				smoothZoom += 0.01f * (InputManager.Any.Horizontal.Value + InputManager.Any.Vertical.Value);
				smoothZoom = Calc.Clamp(smoothZoom, -3f, 3f);
				MDraw.Camera.Zoom = (float)Math.Pow(10, smoothZoom);
			}
			if (ButtonPan.Pressed) {
				MDraw.Camera.WorldPosition.X += 0.3f * InputManager.Any.Horizontal.Value;
				MDraw.Camera.WorldPosition.Y += 0.3f * InputManager.Any.Vertical.Value;
			}
			if (ButtonReset.Pressed) {
				Console.WriteLine("Reset camera to origin");
				MDraw.Camera.Zoom = 1;
				smoothZoom = 0;
				MDraw.Camera.WorldPosition = Vector2.Zero;
			}
		}

	}

}