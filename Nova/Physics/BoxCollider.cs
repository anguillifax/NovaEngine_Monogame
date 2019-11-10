using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Implemented as an Axis Aligned Bounding Box
	/// </summary>
	public class BoxCollider : Component {

		public static implicit operator AABB(BoxCollider col) {
			return new AABB(col.Position, col.Extents);
		}

		/// <summary>
		/// Centerpoint in local space.
		/// </summary>
		public Vector2 LocalPosition { get; set; }

		/// <summary>
		/// Centerpoint in world space.
		/// </summary>
		public Vector2 Position => Entity.Position + LocalPosition;

		/// <summary>
		/// How far each side extends from the centerpoint.
		/// </summary>
		public Vector2 Extents { get; set; }
		/// <summary>
		/// Minimum corner in world space.
		/// </summary>
		public Vector2 Min => Position - Extents;

		/// <summary>
		/// Maximum corner in world space.
		/// </summary>
		public Vector2 Max => Position + Extents;

		/// <summary>
		/// Current velocity of the collider. Returns 0 if not attached to rigidbody.
		/// </summary>
		public Vector2 Velocity => Rigidbody != null ? Rigidbody.Velocity : Vector2.Zero;

		/// <summary>
		/// The parent rigidbody that owns this collider.
		/// </summary>
		public Rigidbody Rigidbody { get; set; }

		public BoxCollider(Entity parent, Vector2 localPosition, Vector2 size) :
			base(parent) {

			LocalPosition = localPosition;
			Extents = size / 2;

			Physics.AllColliders.Add(this);
		}

		public override void Draw() {
			if (Physics.DebugDrawColliders) {
				MDraw.DrawBox(Position, Extents, Physics.ColliderDrawColor);
				MDraw.WriteTiny(Position.ToStringFixed(2), Position, new Color(1, 1, 1, 0.3f));
			}
		}

		public override string ToString() {
			return string.Format("BoxCollider({0} {1})", Position, Extents);
		}

	}

}