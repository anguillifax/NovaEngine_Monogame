using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using Nova.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class SolidFloor : Entity {

		Solid solid;
		BoxCollider boxCollider;
		SpriteRenderer spriteRenderer;

		public SolidFloor(Scene scene, Texture2D texture, Vector2 pos, Vector2 dimensions) :
			base(scene, pos) {

			Scale = dimensions;
			Position = pos;

			spriteRenderer = new SpriteRenderer(this, texture, MDraw.DepthStartScene + 10);

			boxCollider = new BoxCollider(this, Vector2.Zero, dimensions);
			solid = new Solid(this, boxCollider);

			Physics.AllSolids.Add(solid);
		}

		public override void Update() {

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);

			if (InputManager.Any.Unleash.Pressed) {
				Position += 0.1f * vel;
			}
			base.Update();
		}

	}

}