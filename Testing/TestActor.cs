using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestActor : Entity {

		public float speed = 1f;

		ActorRigidbody actor;
		BoxCollider boxCollider;

		public TestActor(Scene scene, Vector2 position, Texture2D texture) :
			base(scene, position) {

			new SpriteRenderer(this, texture, MDraw.DepthStartScene + 0);

			boxCollider = new BoxCollider(this, Vector2.Zero, new Vector2(1f, 1f));
			actor = new ActorRigidbody(this, boxCollider);
		}

		public override void Update() {

			if (InputManager.Any.Jump.JustPressed) {
				actor.Velocity = Vector2.Zero;
			}

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			if (InputManager.Any.Attack.Pressed) {
				Position += 8f * vel * Time.DeltaTime;
				actor.Velocity = Vector2.Zero;
			} else if (InputManager.Any.Unleash.Released) {
				actor.Velocity += .2f * vel * Time.DeltaTime;
			}

			base.Update();
		}

	}

}