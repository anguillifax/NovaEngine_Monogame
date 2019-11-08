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

		private readonly SimpleButton timeDown = new SimpleButton(Microsoft.Xna.Framework.Input.Keys.OemOpenBrackets);
		private readonly SimpleButton timeUp = new SimpleButton(Microsoft.Xna.Framework.Input.Keys.OemCloseBrackets);

		public float speed = 1f;

		BoxCollider b1;
		BoxCollider b2;

		AABB mainA;
		AABB altA, altB;

		public DebugTestPhysics(Scene scene) :
			base(scene, Vector2.Zero) {

			//b1 = new BoxCollider(this, Vector2.Zero, Vector2.One);
			//b2 = new BoxCollider(this, new Vector2(-2f, 2f), new Vector2(1, 1));

			mainA = new AABB(new Vector2(1.75f, 2), 0.5f * Vector2.One);
			vel2 = mainA.Position;

			altA = new AABB(new Vector2(0, 1), new Vector2(0.5f, 0.5f));
			altB = new AABB(new Vector2(0.5f, 0), new Vector2(1f, 0.5f));

		}

		Vector2 Offscreen = new Vector2(float.MaxValue, float.MaxValue);

		Vector2 vel1 = Vector2.Zero;
		Vector2 vel2 = Vector2.Zero;
		Vector2 vel3 = Vector2.Zero;

		float maxtime = 1;

		public override void Update() {

			UpdateControlVariable();
			CustomUpdate();

			base.Update();
		}

		private void UpdateControlVariable() {

			Vector2 vel = new Vector2(InputManager.Any.Horizontal.Value, InputManager.Any.Vertical.Value);
			Calc.ClampMagnitude(ref vel, 1f);

			float changeTimeSpeed = 0.03f;
			if (timeUp.Pressed) maxtime = Math.Min(1, maxtime + changeTimeSpeed);
			if (timeDown.Pressed) maxtime = Math.Max(0, maxtime - changeTimeSpeed);

			if (InputManager.Any.Unleash.Released && InputManager.Any.Attack.Released) {
				// None pressed
				vel1 += 2f * vel * Time.DeltaTime;
				//vel1 += 3f * (new Vector2(-1, 1)).GetNormalized() * vel.X * Time.DeltaTime;
			}
			if (InputManager.Any.Unleash.Released && InputManager.Any.Attack.Pressed) {
				// Attack pressed
				vel2 += 3f * vel * Time.DeltaTime;
			}
			if (InputManager.Any.Unleash.Pressed && InputManager.Any.Attack.Released) {
				// Unleash pressed
				vel3 += 3f * vel * Time.DeltaTime;
			}

		}

		Vector2 slideDelta;
		Vector2 normal;

		private void CustomUpdate() {
			mainA.Position = vel2;

			//slideDelta = PhysicsMath.RecursiveScoop(new AABB[] { altA, altB }, mainA, vel1, Vector2.Zero);
			//Console.WriteLine($"||| {slideDelta}");
		}

		public override void Draw() {

			float cval = 0.2f;
			Color sweepColor = new Color(cval, cval, cval);
			Color endColor = Color.Yellow.Multiply(0.3f);

			DrawSweeps(altA.Position, altA.Position + vel1, altA.Extents, sweepColor, endColor);
			DrawSweeps(altB.Position, altB.Position + vel1, altB.Extents, sweepColor, endColor);

			mainA.Draw(Color.Orchid);
			altA.Draw(Color.Yellow);
			altB.Draw(Color.Yellow);
			mainA.Draw(slideDelta, Color.Cyan);

			base.Draw();
		}

		private void DrawSweeps(Vector2 p1, Vector2 p2, Vector2 extents, Color lineColor, Color endColor) {
			var cur = extents.Clone();
			MDraw.DrawLine(p1 + cur, p2 + cur, lineColor);
			cur.X *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, lineColor);
			cur.Y *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, lineColor);
			cur.X *= -1;
			MDraw.DrawLine(p1 + cur, p2 + cur, lineColor);

			MDraw.DrawBox(p2, extents, endColor);
		}

	}

}