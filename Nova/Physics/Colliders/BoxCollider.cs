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

		public Vector2 LocalPosition { get; set; }
		public Vector2 Position => LocalPosition + Entity.Position;

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
				return PhysicsCollisions.BoxAgainstBox(this, b);
			}
			if (other is CircleCollider c) {
				return PhysicsCollisions.BoxAgainstCircle(this, c);
			}
			return false;
		}

		protected override void DrawCollider() {
			MDraw.DrawBox(Position, Extents, Physics.ColliderDrawColor);
		}

	}

}