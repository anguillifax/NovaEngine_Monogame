using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Implemented as an Axis Aligned Bounding Box
	/// </summary>
	public class BoxCollider : Component {

		public Vector2 LocalPosition;
		public Vector2 Position => Entity.Position + LocalPosition;

		public Vector2 Extents { get; set; }
		public Vector2 Size {
			get { return Extents * 2; }
			set { Extents = value / 2; }
		}

		public Vector2 Min => Position - Extents;
		public Vector2 Max => Position + Extents;

		public Vector2 Velocity => Rigidbody != null ? Rigidbody.Velocity : Vector2.Zero;
		public Rigidbody Rigidbody { get; set; }

		public BoxCollider(Entity parent, Vector2 localPosition, Vector2 size) :
			base(parent) {

			Physics.AllColliders.Add(this);

			LocalPosition = localPosition;
			Size = size;
		}

		public override void Draw() {
			if (Physics.DebugDrawColliders) {
				MDraw.DrawBox(Position, Extents, Physics.ColliderDrawColor);
			}
		}

		public override string ToString() {
			return string.Format("BoxCollider <pos = {0}, extents = {1}>", Position, Extents);
		}

	}

}