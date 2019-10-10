using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public abstract class PhysicsObject {

		/// <summary>
		/// Represents the desired movement during the next physics
		/// </summary>
		public Vector2 Velocity { get; set; }

		public readonly List<Collider> Colliders = new List<Collider>();

	}

}