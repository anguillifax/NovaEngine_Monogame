using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestEntity : Entity {

		public float speed = 1f;

		Actor actor;
		BoxCollider boxCollider;
		SpriteRenderer spriteRenderer;

		public TestEntity(Scene scene, Vector2 position, Texture2D texture) :
			base(scene, position) {

			spriteRenderer = new SpriteRenderer(this, texture, MDraw.DepthStartScene + 0);

			boxCollider = new BoxCollider(this, Vector2.Zero, Vector2.One);
			actor = new Actor(this, boxCollider);
		}

		public override void Update() {

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			actor.Velocity = 8 * vel * Time.DeltaTime;

			base.Update();
		}

		public override void Draw() {
			base.Draw();
		}

	}

}