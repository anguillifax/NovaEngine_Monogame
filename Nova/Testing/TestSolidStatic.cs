using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;

namespace Nova {

	public class TestSolidStatic : Entity {

		public TestSolidStatic(Scene scene, Texture2D texture, Vector2 pos, Vector2 dimensions) :
			base(scene, pos) {

			Scale = dimensions;

			new SpriteRenderer(this, texture, MDraw.DepthStartScene + 11);

			new StaticRigidbody(this, new BoxCollider(this, Vector2.Zero, dimensions));

			SetPosition(pos);

		}

		public void SetPosition(Vector2 newPos) {
			Position = newPos;
			// TODO: Recalculate region
		}

	}

}