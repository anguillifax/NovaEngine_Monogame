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
	public static class PhysicsMath {

		public static float Epsilon = 1e-15f;

		#region Overlap XX against XX

		public static bool OverlapBoxAgainstBox(BoxCollider a, BoxCollider b) {
			if (CannotCollide(a, b)) return false;

			if (Math.Abs(a.Position.X - b.Position.X) > a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) > a.Extents.Y + b.Extents.Y) return false;
			return true;
		}

		public static bool OverlapCircleAgainstCircle(CircleCollider a, CircleCollider b) {
			if (CannotCollide(a, b)) return false;

			Vector2 d = a.Position - b.Position;
			float distSquared = Vector2.Dot(d, d);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum * radiusSum;
		}

		public static bool OverlapBoxAgainstCircle(BoxCollider box, CircleCollider circle) {
			if (CannotCollide(box, circle)) return false;

			return SquareDistanceBetweenPointAndBox(circle.Position, box) <= circle.Radius * circle.Radius;
		}

		#endregion

		#region Closest Point and Distance Mathematics

		/// <summary>
		/// Returns the closest point on the boundary of the box or inside the box.
		/// </summary>
		public static Vector2 ClosestPointOnBox(Vector2 point, BoxCollider box) {
			var min = box.Min;
			var max = box.Max;

			if (point.X < min.X) point.X = min.X;
			if (point.X > max.X) point.X = max.X;

			if (point.Y < min.Y) point.Y = min.Y;
			if (point.Y > max.Y) point.Y = max.Y;

			return point;
		}

		public static float SquareDistanceBetweenPointAndBox(Vector2 point, BoxCollider box) {
			float sqDist = 0f;

			var min = box.Min;
			var max = box.Max;

			if (point.X < min.X) sqDist += Squared(min.X - point.X);
			if (point.X > max.X) sqDist = Squared(point.X - max.X);

			if (point.Y < min.Y) sqDist += Squared(min.Y - point.Y);
			if (point.Y > max.Y) sqDist = Squared(point.Y - max.Y);

			return sqDist;
		}

		#endregion

		#region Moving Objects

		public static bool IntersectMovingBoxAgainstBox(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact, out float lastTimeOfContact) {

			if (OverlapBoxAgainstBox(a, b)) {
				firstTimeOfContact = lastTimeOfContact = 0f;
				return true;
			}

			Vector2 v = velB - velA;

			firstTimeOfContact = 0f;
			lastTimeOfContact = 1f;

			if (v.LengthSquared() == 0) {
				return false;
			}

			if (v.X < 0f) {
				if (b.Max.X < a.Min.X) return false;
				if (a.Max.X < b.Min.X) firstTimeOfContact = Math.Max((a.Max.X - b.Min.X) / v.X, firstTimeOfContact);
				if (b.Max.X > a.Min.X) lastTimeOfContact = Math.Min((a.Min.X - b.Max.X) / v.X, lastTimeOfContact);
			}
			if (v.X > 0f) {
				if (b.Min.X > a.Max.X) return false;
				if (b.Max.X < a.Min.X) firstTimeOfContact = Math.Max((a.Min.X - b.Max.X) / v.X, firstTimeOfContact);
				if (a.Max.X > b.Min.X) lastTimeOfContact = Math.Min((a.Max.X - b.Min.X) / v.X, lastTimeOfContact);
			}

			if (firstTimeOfContact > lastTimeOfContact) return false;


			if (v.Y < 0f) {
				if (b.Max.Y < a.Min.Y) return false;
				if (a.Max.Y < b.Min.Y) firstTimeOfContact = Math.Max((a.Max.Y - b.Min.Y) / v.Y, firstTimeOfContact);
				if (b.Max.Y > a.Min.Y) lastTimeOfContact = Math.Min((a.Min.Y - b.Max.Y) / v.Y, lastTimeOfContact);
			}
			if (v.Y > 0f) {
				if (b.Min.Y > a.Max.Y) return false;
				if (b.Max.Y < a.Min.Y) firstTimeOfContact = Math.Max((a.Min.Y - b.Max.Y) / v.Y, firstTimeOfContact);
				if (a.Max.Y > b.Min.Y) lastTimeOfContact = Math.Min((a.Max.Y - b.Min.Y) / v.Y, lastTimeOfContact);
			}

			if (firstTimeOfContact > lastTimeOfContact) return false;

			return true;
		}

		// Circle against circle p225

		// Circle against box p229

		#endregion

		#region Raycasts (TODO)

		// Sphere p217

		// Box p219

		#endregion

		/// <summary>
		/// Returns true if given colliders are null or inactive.
		/// </summary>
		private static bool CannotCollide(Collider a, Collider b) {
			return a == null || b == null || !a.Active || !b.Active;
		}

		/// <summary>
		/// Returns the squared value of v. Created exclusively to help improve mathematical
		/// clarity within this class.
		/// </summary>
		private static float Squared(float v) {
			return v * v;
		}

	}

}