using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Implemented as an Axis Aligned Bounding Box
	/// </summary>
	public class BoxCollider : Collider {

		public Vector2 Min => Position - Extents;
		public Vector2 Max => Position + Extents;

		public Vector2 Extents { get; set; }

		public Vector2 Size {
			get { return Extents * 2; }
			set { Extents = value / 2; }
		}

		public BoxCollider(Entity parent, Vector2 localPosition, Vector2 size) :
			base(parent) {

			LocalPosition = localPosition;
			Size = size;
		}

		public override bool Collide(Collider other) {
			if (other is BoxCollider b) {
				return PhysicsMath.IsOverlapping_Box_Box(this, b);
			}
			if (other is CircleCollider c) {
				return PhysicsMath.IsOverlapping_Box_Circle(this, c);
			}
			return false;
		}

		protected override void DrawCollider() {
			MDraw.DrawBox(Position, Extents, Physics.ColliderDrawColor);
		}

		public override string ToString() {
			return string.Format("BoxCollider <pos = {0}, extents = {1}>", Position, Extents);
		}

	}

}