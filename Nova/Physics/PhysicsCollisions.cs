using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Defines the mathematics behind all possible collisions between different types of colliders.
	/// Adapted from Christer Ericson's Real-Time Collision Book.
	/// </summary>
	public static class PhysicsCollisions {

		public static bool BoxAgainstBox(BoxCollider a, BoxCollider b) {
			if (CannotCollide(a, b)) return false;

			if (Math.Abs(a.Position.X - b.Position.X) > a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) > a.Extents.Y + b.Extents.Y) return false;
			return true;
		}

		public static bool CircleAgainstCircle(CircleCollider a, CircleCollider b) {
			if (CannotCollide(a, b)) return false;

			Vector2 d = a.Position - b.Position;
			float distSquared = Vector2.Dot(d, d);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum * radiusSum;
		}

		public static bool BoxAgainstCircle(BoxCollider box, CircleCollider circle) {
			if (CannotCollide(box, circle)) return false;

			Console.WriteLine("Not implemented!");

			return false;
		}

		/// <summary>
		/// Returns true if given colliders are null or inactive.
		/// </summary>
		private static bool CannotCollide(Collider a, Collider b) {
			return a == null || b == null || !a.Active || !b.Active;
		}

	}

}