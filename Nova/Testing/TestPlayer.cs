using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestPlayer : Entity {

		public float MaxSpeed { get; set; }
		public float Acceleration { get; set; }
		public float DecelMultiplier { get; set; }
		public Vector2 Velocity;

		SpriteRenderer spriteRenderer;
		//BoxCollider collider;

		public TestPlayer(Scene s, Texture2D tex) :
			base(s, Vector2.Zero) {
			spriteRenderer = new SpriteRenderer(this, tex, MDraw.DepthStartScene + 0);
			//collider = new BoxCollider(this, new Vector2(0, -0.1f), new Vector2(.6f, 1.5f));

			MaxSpeed = 14;
			Acceleration = MaxSpeed / 0.2f;
			DecelMultiplier = 0.8f;
		}

		public override void Update() {
			var inputs = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);

			// Snap directions
			if (Velocity.X * inputs.X < 0) {
				Velocity.X = 0;
			}

			// Core movement
			if (inputs.X != 0) {
				// Linear ramp up to max speed
				Velocity.X += inputs.X * Acceleration * Time.DeltaTime;
				Velocity.X = Calc.Clamp(Velocity.X, -MaxSpeed, MaxSpeed);
			} else {
				// Exponential ramp to zero
				if (Math.Abs(Velocity.X) > 0.1f) {
					Velocity.X *= DecelMultiplier;
				} else {
					Velocity.X = 0;
				}
			}

			Position += Velocity * Time.DeltaTime;

			base.Update();
		}

		public override void Draw() {
			spriteRenderer.Draw();
		}

	}

}