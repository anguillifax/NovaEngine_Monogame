using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Input;
using Nova.PhysicsEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	/// <summary>
	/// Temporary class used to test physics math before integration.
	/// </summary>
	public class DebugTestPhysics : Entity {

		SimpleButton timeDown = new SimpleButton(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets);
		SimpleButton timeUp = new SimpleButton(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets);

		public float speed = 1f;

		ActorRigidbody actor;
		SolidRigidbody solid;
		BoxCollider b1;
		BoxCollider b2;

		public DebugTestPhysics(Scene scene) :
			base(scene, Vector2.Zero) {

			b1 = new BoxCollider(this, Vector2.Zero, Vector2.One);
			b2 = new BoxCollider(this, new Vector2(-1.5f, 0.2f), new Vector2(1, 1));
  
		}

		Vector2 slideDelta;
		Vector2 testVel = Vector2.Zero;
		float maxtime = 1;

		public override void Update() {

			if (InputManager.Any.Jump.JustPressed) {
				testVel = Vector2.Zero;
			}

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			float timeSpeed = 0.03f;
			if (timeUp.Pressed) maxtime = Math.Min(1, maxtime + timeSpeed);
			if (timeDown.Pressed) maxtime = Math.Max(0, maxtime - timeSpeed);

			if (InputManager.Any.Unleash.Released) {
				testVel += 3f * vel * Time.DeltaTime;
			}


			//Console.WriteLine(PhysicsMath.IsInMovementPath(b1, b2, Vector2.UnitY, testVel));
			Console.WriteLine(Vector2.Dot(b1.Position - b2.Position, Vector2.UnitY - Vector2.UnitY));

			base.Update();
		}

		public override void Draw() {

			var p1 = b1.Position;
			var p2 = b1.Position + testVel;

			float cval = 0.2f;
			Color sweeps = new Color(cval, cval, cval);

			var cur = b1.Extents.Clone();
			MDraw.DrawLine(p1 + cur, p2 + cur, sweeps);
			cur.X *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, sweeps);
			cur.Y *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, sweeps);
			cur.X *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, sweeps);

			MDraw.DrawBox(b1.Position + testVel, b1.Extents, Color.Gray);
			MDraw.DrawBox(b1.Position + testVel * maxtime, b1.Extents, Color.White);
			MDraw.DrawBox(b1.Position + slideDelta, b2.Extents, Color.Cyan);

			base.Draw();
		}

	}

}