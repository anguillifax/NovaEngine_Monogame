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
			if (Math.Abs(a.Position.X - b.Position.X) > a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) > a.Extents.Y + b.Extents.Y) return false;
			return true;
		}

		public static bool OverlapCircleAgainstCircle(CircleCollider a, CircleCollider b) {
			Vector2 d = a.Position - b.Position;
			float distSquared = Vector2.Dot(d, d);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum * radiusSum;
		}

		public static bool OverlapBoxAgainstCircle(BoxCollider box, CircleCollider circle) {
			return SquareDistanceBetweenPointAndBox(circle.Position, box) <= circle.Radius * circle.Radius;
		}

		#endregion

		#region Closest Point and Distance Mathematics

		/// <summary>
		/// Returns the closest point on the boundary of the box or inside the box.
		/// </summary>
		public static Vector2 ClosestPointOnBox(Vector2 point, BoxCollider box) {
			if (point.X < box.Min.X) point.X = box.Min.X;
			if (point.X > box.Max.X) point.X = box.Max.X;

			if (point.Y < box.Min.Y) point.Y = box.Min.Y;
			if (point.Y > box.Max.Y) point.Y = box.Max.Y;

			return point;
		}

		public static float SquareDistanceBetweenPointAndBox(Vector2 point, BoxCollider box) {
			float sqDist = 0f;

			if (point.X < box.Min.X) sqDist += Squared(box.Min.X - point.X);
			if (point.X > box.Max.X) sqDist = Squared(point.X - box.Max.X);

			if (point.Y < box.Min.Y) sqDist += Squared(box.Min.Y - point.Y);
			if (point.Y > box.Max.Y) sqDist = Squared(point.Y - box.Max.Y);

			return sqDist;
		}

		/// <summary>
		/// Returns a point on a plane defined by normal and passing through the origin.
		/// </summary>
		public static Vector2 GetClosestPointOnOriginPlane(Vector2 point, Vector2 normal) {
			normal.Normalize();
			return point - Vector2.Dot(normal, point) * normal;
		}

		#endregion

		#region Normals

		public static Vector2 GetClosestNormalOnBox(Vector2 point, BoxCollider box) {

			Vector2 normal = Vector2.Zero;

			if (point.X < box.Position.X) normal.X = -1;
			if (point.X > box.Position.X) normal.X = 1;

			if (point.Y < box.Position.Y) normal.Y = -1;
			if (point.Y > box.Position.Y) normal.Y = 1;

			return normal.LengthSquared() == 0 ? Vector2.UnitX : normal.GetNormalized();
		}

		public static Vector2 GetClosestNormalOnCircle(Vector2 point, CircleCollider circle) {
			var normal = point - circle.Position;
			return normal.LengthSquared() == 0 ? Vector2.UnitX : normal.GetNormalized();
		}

		#endregion

		#region Moving Objects

		/// <summary>
		/// Returns time when two moving boxes overlap. If A overlaps B, then it is considered an intersection.
		/// </summary>
		public static bool IntersectMovingBoxAgainstBox(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {
			if (OverlapBoxAgainstBox(a, b)) {
				firstTimeOfContact = 0f;
				return true;
			} else {
				return IntersectMovingBoxAgainstBoxNoOverlap(a, b, velA, velB, out firstTimeOfContact);
			}
		}

		/// <summary>
		/// Returns time when two moving boxes overlap. If A overlaps B, then it is NOT considered an intersection.
		/// </summary>
		public static bool IntersectMovingBoxAgainstBoxNoOverlap(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {

			// If A is stationary, v is relative velocity of B
			Vector2 v = velB - velA;

			firstTimeOfContact = 1f;
			bool hit = false;

			if (v.X != 0) {

				if (b.Max.X <= a.Min.X && v.X < 0f) {
					// B is moving away from A to the left. no intersection
					return false;

				} else if (b.Min.X >= a.Max.X && v.X > 0f) {
					// B is moving away from A to the right. no intersection
					return false;

				} else {
					// B is approaching A on x axis. possible intersection

					float t;
					if (v.X > 0f) {
						t = (a.Min.X - b.Max.X) / v.X;
					} else {
						t = (a.Max.X - b.Min.X) / v.X;
					}

					if (0f <= t && t <= 1f) {

						float projectionY = v.Y * t;

						if (b.Min.Y + projectionY < a.Max.Y && b.Max.Y + projectionY > a.Min.Y) {
							// y component of projection will overlap if B moves to time t
							firstTimeOfContact = Math.Min(t, firstTimeOfContact);
							hit = true;
						}

					}

				}

			}

			if (v.Y != 0) {

				if (b.Max.Y <= a.Min.Y && v.Y < 0f) {
					// B is moving away from A downwards. no intersection
					return false;

				} else if (b.Min.Y >= a.Max.Y && v.Y > 0f) {
					// B is moving away from A upwards. no intersection
					return false;

				} else {
					// B is approaching A on y axis. possible intersection

					float t;

					if (v.Y > 0f) {
						t = (a.Min.Y - b.Max.Y) / v.Y;
					} else {
						t = (a.Max.Y - b.Min.Y) / v.Y;
					}

					if (0f <= t && t <= 1f) {

						float projectionX = v.X * t;

						if (a.Min.X < b.Max.X + projectionX && a.Max.X > b.Min.X + projectionX) {
							// x component of projection will overlap if B moves to time t
							firstTimeOfContact = Math.Min(t, firstTimeOfContact);
							hit = true;
						}

					}

				}

			}

			return hit;
		}

		// Circle against circle p225

		// Circle against box p229

		#endregion

		#region Overlap Resolution (Depenetration)

		/// <summary>
		/// Calculate the closest point that puts A outside of B. Do not use if A is not overlapping B.
		/// </summary>
		public static Vector2 DepenetrateBoxAgainstBox(BoxCollider a, BoxCollider b) {

			if (a.Position == b.Position) {
				// Objects are on top of each other. Push A above B.
				return b.Position + new Vector2(0, b.Extents.Y + a.Extents.Y);
			}

			Vector2 delta = a.Position - b.Position;

			if (Math.Abs(delta.X) >= Math.Abs(delta.Y)) {
				// A is farther from center in horizontal direction
				return b.Position + new Vector2(Math.Sign(delta.X) * (b.Extents.X + a.Extents.X), delta.Y);

			} else {
				// A is farther from center in vertical direction
				return b.Position + new Vector2(delta.X, Math.Sign(delta.Y) * (b.Extents.Y + a.Extents.Y));
			}
		}

		#endregion

		#region Raycasts (TODO)

		// Sphere p217

		// Box p219

		#endregion

		#region Specialized

		public static Vector2 GetAllowedVelocity(Vector2 vel, Vector2 normal) {

			normal.Normalize();
			var dot = Vector2.Dot(vel, normal);

			if (dot < 0) {
				// point is below plane. snap to plane
				return vel - dot * normal;
			} else {
				return vel;
			}
		}

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