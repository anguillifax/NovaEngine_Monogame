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
		BoxCollider boxCollider, b2;
		SpriteRenderer spriteRenderer;

		public TestEntity(Scene scene, Vector2 position, Texture2D texture) :
			base(scene, position) {

			spriteRenderer = new SpriteRenderer(this, texture, MDraw.DepthStartScene + 0);

			boxCollider = new BoxCollider(this, Vector2.Zero, Vector2.One);
			actor = new Actor(this, boxCollider);

			Physics.AllActors.Add(actor);

			//b2 = new BoxCollider(this, new Vector2(4, 0), Vector2.One);
		}

		Vector2 testVel = new Vector2();

		public override void Update() {

			if (InputManager.Any.Jump.JustPressed) {
				Position = Vector2.Zero;
				//b2.LocalPosition = Vector2.Zero;
				testVel = Vector2.Zero;
				actor.Velocity = Vector2.Zero;
			}

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			//actor.Velocity = 3f * vel * Time.DeltaTime;

			if (InputManager.Any.Attack.Pressed) {
				Position += 8f * vel * Time.DeltaTime;
				actor.Velocity = Vector2.Zero;
			} else if (InputManager.Any.Unleash.Released) {
				actor.Velocity += .2f * vel * Time.DeltaTime;
				//actor.Velocity = 3f * vel * Time.DeltaTime;
			}
			//else if (InputManager.Any.Unleash.Pressed) {
			//b2.LocalPosition += 0.1f * vel;
			//} else {
			//	testVel += 0.1f * vel;
			//}


			//if (InputManager.Any.Jump.JustPressed) {
			//	boxCollider.LocalPosition += new Vector2(0, 0.3f);
			//}

			base.Update();
		}

		public override void Draw() {

			//if (PhysicsMath.IntersectMovingBoxAgainstBox(boxCollider, b2, testVel, Vector2.Zero, out float first, out float last)) {
			//	MDraw.DrawPoint(Position + testVel * first, Color.White);
			//	MDraw.DrawPoint(Position + testVel * last, Color.Yellow);
			//} else {
			//	MDraw.DrawPoint(Position + testVel, Color.White);
			//}

			base.Draw();
		}

	}

}