using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using Nova.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestSolid : Entity {

		Solid solid;
		BoxCollider boxCollider;

		Vector2 initPos;

		public TestSolid(Scene scene, Texture2D texture, Vector2 pos, Vector2 dimensions) :
			base(scene, pos) {

			Scale = dimensions;
			Position = pos;

			initPos = Position;

			new SpriteRenderer(this, texture, MDraw.DepthStartScene + 10);

			boxCollider = new BoxCollider(this, Vector2.Zero, dimensions);
			solid = new Solid(this, boxCollider);
		}

		public override void Update() {

			float scalar = MathHelper.TwoPi / 2f;

			float y = 2 / (10 * scalar) * (float)Math.Cos(Time.TotalTime * scalar);
			//solid.Velocity = new Vector2(0, 1 * Time.DeltaTime);

			if (Position.Y > 6) {
				Position.Y = -2;
			}
			
			base.Update();
		}

	}

}