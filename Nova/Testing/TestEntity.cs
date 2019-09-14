using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestEntity : VisualEntity {

		SpriteRenderer spriteRenderer;

		public TestEntity(Scene s, Texture2D tex) :
			base(s) {
			spriteRenderer = new SpriteRenderer(this, tex);
		}

		public override void Update() {
			var inputs = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Position += Calc.ClampMagnitude(10 * inputs, 10);
			base.Update();
		}

		public override void Draw() {
			spriteRenderer.Render();
			MDraw.Begin();
			MDraw.Write("Henlo", Position + new Vector2(100f, 100f), Color.White);
			MDraw.End();
		}

	}

}