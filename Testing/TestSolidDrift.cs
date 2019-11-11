using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Input;
using Nova.PhysicsEngine;

namespace Nova {

	public class TestSolidDrift : Entity {

		SimpleButton btn = new SimpleButton(Microsoft.Xna.Framework.Input.Keys.Y);

		SolidRigidbody rb;
		Vector2 initpos;

		public TestSolidDrift(Scene scene, Texture2D texture, Vector2 pos, Vector2 dimensions) :
			base(scene, pos) {

			Scale = dimensions;
			initpos = pos;

			new SpriteRenderer(this, texture, MDraw.DepthStartScene + 11);

			rb = new SolidRigidbody(this, new BoxCollider(this, Vector2.Zero, dimensions));

		}

		public override void Update() {
			if (btn.JustPressed) {
				System.Console.WriteLine("starting slide");

				Position = initpos;
				rb.Velocity = new Vector2(-0.4f, 0f) * Time.DeltaTime;
			}
			base.Update();
		}

	}

}