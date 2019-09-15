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
			Scale = new Vector2(0.25f, 0.25f);
		}

		public override void Update() {
			var inputs = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Position += Calc.ClampMagnitude(10 * inputs, 10);
			base.Update();
		}

		public override void Draw() {
			spriteRenderer.Render();
			MDraw.Begin();
			//MDraw.Write("Henlo", Position + new Vector2(100f, 100f), Color.White);
			//MDraw.DrawBox(Vector2.Zero, Position, Color.Yellow);
			//MDraw.DrawNGon(Vector2.Zero, 1 * Time.TotalTime, (int)(Time.TotalTime + 3), Color.Lime, MathHelper.PiOver4 * Time.TotalTime);
			//MDraw.DrawBox(Vector2.Zero, new Vector2(100), Color.Magenta);
			MDraw.DrawCircle(Vector2.Zero, 100, Color.Cyan);
			//MDraw.DrawCircleGlobal(Screen.Center, 10, Color.CornflowerBlue);
			//MDraw.DrawLine(Vector2.Zero, new Vector2(10f), Color.Orange);
			MDraw.End();
		}

	}

}