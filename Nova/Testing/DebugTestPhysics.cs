using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	/// <summary>
	/// Temporary class used to test physics math before integration.
	/// </summary>
	public class DebugTestPhysics : Entity {

		public float speed = 1f;

		Actor actor;
		Solid solid;
		BoxCollider b1;
		BoxCollider b2;

		public DebugTestPhysics(Scene scene) :
			base(scene, Vector2.Zero) {

			b1 = new BoxCollider(this, Vector2.Zero, Vector2.One);
			b2 = new BoxCollider(this, new Vector2(0.999999f, 0), new Vector2(1, 1));

		}

		Vector2 pushDelta;
		Vector2 testVel = Vector2.Zero;

		public override void Update() {

			if (InputManager.Any.Jump.JustPressed) {
				testVel = Vector2.Zero;
			}

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			if (InputManager.Any.Unleash.Released) {
				testVel += 3f * vel * Time.DeltaTime;
			}

			b2.LocalPosition = testVel;
			pushDelta = PhysicsMath.GetNormal(b2, b1);

			//if (PhysicsMath.IntersectPushFuzzy(b1, b2, testVel, out Vector2 delta)) {
			//	pushPos = delta;
			//} else {
			//	pushPos = Vector2.Zero;
			//}

			//if (PhysicsMath.IntersectMovingNoOverlapFuzzy(b1, b2, testVel, Vector2.Zero, out float t)) {
			//	pushDelta = testVel * t;
			//} else {
			//	pushDelta = 1000 * Vector2.One;
			//}



			base.Update();
		}

		public override void Draw() {

			var p1 = b1.Position;
			var p2 = b1.Position + testVel;

			//var cur = b1.Extents.Clone();
			//MDraw.DrawLine(p1 + cur, p2 + cur, Color.DimGray);
			//cur.X *= -1;
			//MDraw.DrawLine(p1 + cur, p2 + cur, Color.DimGray);
			//cur.Y *= -1;
			//MDraw.DrawLine(p1 + cur, p2 + cur, Color.DimGray);
			//cur.X *= -1;
			//MDraw.DrawLine(p1 + cur, p2 + cur, Color.DimGray);

			MDraw.DrawBox(b1.Position + testVel, b1.Extents, Color.White);
			MDraw.DrawLine(Vector2.Zero, pushDelta,  Color.Cyan);

			base.Draw();
		}

	}

}