using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public abstract class Collider : Component {

		public Vector2 LocalPosition;
		public Vector2 Position => Entity.Position + LocalPosition;

		public Rigidbody Rigidbody { get; set; }
		public Vector2 Velocity => Rigidbody != null ? Rigidbody.Velocity : Vector2.Zero;

		public Collider(Entity parent) : base(parent) {
			Physics.AllColliders.Add(this);
		}

		public abstract bool Collide(Collider other);

		public sealed override void Draw() {
			if (Physics.DebugDrawColliders) {
				DrawCollider();
			}
		}

		protected abstract void DrawCollider();

	}

}