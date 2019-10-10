using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public class CircleCollider : Collider {

		public Vector2 LocalPosition { get; set; }
		public Vector2 Position => LocalPosition + Entity.Position;

		public float Radius { get; set; }

		public CircleCollider(Entity parent, Vector2 localPos, float radius) :
			base(parent) {

			LocalPosition = LocalPosition;
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