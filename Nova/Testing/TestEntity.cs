using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestEntity : Entity {

		SpriteRenderer spriteRenderer;
		BoxCollider collider;

		public TestEntity(Scene s, Texture2D tex) :
			base(s) {
			spriteRenderer = new SpriteRenderer(this, tex);
			collider = new BoxCollider(this, new Vector2(0, -0.1f), new Vector2(.6f, 1.5f));
		}

		public override void Update() {
			var inputs = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Position += Calc.ClampMagnitude(.2f * inputs, .2f);

			if (InputManager.Any.Jump.JustPressed) {
				Console.WriteLine(Position);
				Position = Calc.Round(Position, 1);
			}

			base.Update();
		}

		public override void Draw() {
			spriteRenderer.Render();
			collider.Draw();
		}

	}

}