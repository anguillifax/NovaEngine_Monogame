using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public struct ContactPoint {

		public Collider collider;

		public Vector2 point;
		public Vector2 normal;
		public float time;

		public ContactPoint(Collider collider, Vector2 point, Vector2 normal, float time) {
			this.collider = collider;
			this.point = point;
			this.normal = normal;
			this.time = time;
		}

	}

}