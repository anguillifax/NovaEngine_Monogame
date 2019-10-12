using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public class CircleCollider : Collider {

		public float Radius { get; set; }

		public CircleCollider(Entity parent, Vector2 localPosition, float radius) :
			base(parent) {

			LocalPosition = localPosition;
			Radius = radius;
		}

		public override bool Collide(Collider other) {
			if (other is BoxCollider b) {
				return PhysicsMath.OverlapBoxAgainstCircle(b, this);
			}
			if (other is CircleCollider c) {
				return PhysicsMath.OverlapCircleAgainstCircle(this, c);
			}

			return false;
		}

		protected override void DrawCollider() {
			MDraw.DrawCircle(Position, Radius, Physics.ColliderDrawColor);
		}

	}

}