using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using Nova.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class SolidFloor : Entity {

		BoxCollider boxCollider;
		SpriteRenderer spriteRenderer;

		public SolidFloor(Scene scene, Texture2D texture, Vector2 pos, Vector2 dimensions) :
			base(scene, pos) {

			Scale = dimensions;
			Position = pos;
			spriteRenderer = new SpriteRenderer(this, texture, MDraw.DepthStartScene + 10);
			boxCollider = new BoxCollider(this, Vector2.Zero, Vector2.One);
		}

	}

}