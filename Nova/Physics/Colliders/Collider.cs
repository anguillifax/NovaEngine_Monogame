using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public abstract class Collider : Component {

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