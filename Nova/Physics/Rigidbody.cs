using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public abstract class Rigidbody : Component {

		/// <summary>
		/// Represents the desired movement during the next physics update
		/// </summary>
		public Vector2 Velocity { get; set; }

		public readonly List<Collider> Colliders;

		public Rigidbody(Entity parent, params Collider[] colliders) : base(parent) {
			Colliders = new List<Collider>(colliders);
		}

	}

}