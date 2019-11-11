using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Defines the mathematics and tests underlying the Physics engine. This is where the nitty gritty logic happens.
	/// <para>Partially adapted from Christer Ericson's Real-Time Collision Book, and partially written from scratch.</para>
	/// <para>This math library has no knowledge of Rigidbodies or Colliders. It operates purely on BoxColliders and Vectors.</para>
	/// </summary>
	public static class PhysicsMath {

		/* Comparison operators with custom Epsilon
		 * 
		 * a >= b  :  a + Epsilon >= b
		 * a >  b  :  a - Epsilon >  b
		 *
		 * a <= b  :  a <= b + Epsilon
		 * a <  b  :  a <  b - Epsilon
		 * 
		 * Epsilon always goes on the BIG side of the operator.
		 * ADD Epsilon to make >= or <=
		 * SUBTRACT Epsilon to make > or <
		 *   Note: Feel free to rearrange if meaning is clearer.
		 * 
		 * Use ~< and ~> to indicate "mostly less" and "mostly more"
		 * Use double << or >> to indicate "much less" and "much more"
		 */

		public const float Epsilon = 1e-5f;
		public const float BigEpsilon = 1e-4f;

		#region Overlaps

		/// <summary>
		/// Returns true if A is very nearly overlapping B.
		/// </summary>
		public static bool IsOverlapping(BoxCollider a, BoxCollider b) {

			if (Math.Abs(a.Position.X - b.Position.X) > a.Extents.X + b.Extents.X + Epsilon) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) > a.Extents.Y + b.Extents.Y + Epsilon) return false;
			// fails if separation distance is much larger than extents.

			return true;
		}

		/// <summary>
		/// Tests if A is significantly inside of B. If A and B are aligned on the edges, this function returns false.
		/// </summary>
		public static bool IsInside(BoxCollider a, BoxCollider b) {
			if (Math.Abs(a.Position.X - b.Position.X) >= a.Extents.X + b.Extents.X - Epsilon) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) >= a.Extents.Y + b.Extents.Y - Epsilon) return false;
			// separation distance must be much larger than extents.

			return true;
		}

		/// <summary>
		/// Strictly tests if A is inside of B. If A and B are aligned on the edges, this function returns false.
		/// </summary>
		public static bool IsInsideExact(BoxCollider a, BoxCollider b) {
			if (Math.Abs(a.Position.X - b.Position.X) >= a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) >= a.Extents.Y + b.Extents.Y) return false;
			// separation distance must larger than extents.

			return true;
		}

		#endregion

		#region Intersection Tests

		/// <summary>
		/// Given two moving colliders, calculate when A will touch B. If A initially overlaps B, then it is NOT considered an intersection.
		/// </summary>
		public static bool IntersectMoving(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {

			// If A is stationary, v is relative velocity of B
			Vector2 v = velB - velA;

			firstTimeOfContact = 1f;
			bool hit = false;

			if (v.X != 0) {

				if (v.X < 0f && b.Max.X <= a.Min.X + Epsilon) {
					// B is moving away from A to the left. no intersection
					return false;

				} else if (v.X > 0f && b.Min.X + Epsilon >= a.Max.X) {
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

				if (v.Y < 0f && b.Max.Y <= a.Min.Y + Epsilon) {
					// B is moving away from A downwards. no intersection
					return false;

				} else if (v.Y > 0f && b.Min.Y + Epsilon >= a.Max.Y) {
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

		/// <summary>
		/// Given stationary actor and moving solid, calculate how far the solid pushes the actor.
		/// </summary>
		public static bool IntersectPush(BoxCollider solid, BoxCollider actor, Vector2 solidVel, float maxMoveTime, out Vector2 actorMoveDelta) {

			actorMoveDelta = Vector2.Zero;

			if (solidVel.X != 0) {

				if (solidVel.X < 0f && solid.Max.X <= actor.Min.X + Epsilon) {
					// Solid is moving away from actor to the left. No intersection
					return false;

				} else if (solidVel.X > 0f && solid.Min.X + Epsilon >= actor.Max.X) {
					// Solid is moving away from actor to the right. No intersection
					return false;

				} else {
					// Solid is approaching/touching actor on x axis. Possible intersection

					float timeOfContact;

					// Calculate positive time of contact
					if (solidVel.X > 0f) {
						timeOfContact = (actor.Min.X - solid.Max.X) / solidVel.X;
					} else {
						timeOfContact = (actor.Max.X - solid.Min.X) / solidVel.X;
					}

					// Check if contact in horizontal direction will happen during this frame
					if (IsWithin(-BigEpsilon, timeOfContact, maxMoveTime + BigEpsilon)) {

						// Find the y-position at time of contact
						float projectionY = solidVel.Y * timeOfContact;

						// Push if vertical edge of solid will be touching actor at time of x-contact.
						// Push if corners are exactly aligned in a diagonal push.
						if ((solid.Min.Y + projectionY < actor.Max.Y && solid.Max.Y + projectionY > actor.Min.Y) ||
							IsEqual(solid.Max.Y + projectionY, actor.Min.Y) || IsEqual(solid.Min.Y + projectionY, actor.Max.Y)) {

							if (solidVel.Y == 0) {
								// Pushing horizontally only. push actor remaining distance after contact
								actorMoveDelta = new Vector2(solidVel.X * (maxMoveTime - timeOfContact), 0);
								return true;

							} else {

								float timeOfSeparation;

								// Calculate positive time of separation
								if (solidVel.Y > 0) {
									timeOfSeparation = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
								} else {
									timeOfSeparation = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
								}

								// Take min in case time of separation is greater than maxMoveTime.
								actorMoveDelta = new Vector2(solidVel.X * (Math.Min(maxMoveTime, timeOfSeparation) - timeOfContact), 0);
								return true;

							}

						}

					}

				}

			} // end solidVelocity.X != 0 check

			if (solidVel.Y != 0) {

				if (solidVel.Y < 0f && solid.Max.Y <= actor.Min.Y + Epsilon) {
					// Solid is moving away from actor downward. No intersection
					return false;

				} else if (solidVel.Y > 0f && solid.Min.Y + Epsilon >= actor.Max.Y) {
					// Solid is moving away from actor upward. No intersection
					return false;

				} else {
					// Solid is approaching/touching actor on y axis. Possible intersection

					float timeOfContact;

					// Calculate positive time of contact
					if (solidVel.Y > 0f) {
						timeOfContact = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
					} else {
						timeOfContact = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
					}

					// Check if contact in vertical direction will happen during this frame
					if (IsWithin(-BigEpsilon, timeOfContact, maxMoveTime + BigEpsilon)) {

						// Find the x-position at time of contact
						float projectionX = solidVel.X * timeOfContact;

						// Push if horizontal edge of solid will be touching actor at time of y-contact
						if (solid.Min.X + projectionX < actor.Max.X && solid.Max.X + projectionX > actor.Min.X) {

							if (solidVel.X == 0) {
								// Pushing vertically only. Push actor remaining distance after contact
								actorMoveDelta = new Vector2(0, solidVel.Y * (maxMoveTime - timeOfContact));
								return true;

							} else {
								float timeOfSeparation;

								// Calculate positive time of separation
								if (solidVel.X > 0) {
									timeOfSeparation = (actor.Max.X - solid.Min.X) / solidVel.X;
								} else {
									timeOfSeparation = (actor.Min.X - solid.Max.X) / solidVel.X;
								}

								// Take min in case time of separation is greater than maxMoveTime.
								actorMoveDelta = new Vector2(0, solidVel.Y * (Math.Min(maxMoveTime, timeOfSeparation) - timeOfContact));
								return true;
							}

						}

					}

				}

			} // end solidVelocity.Y != 0 check


			return false;

		}

		/// <summary>
		/// Given two moving colliders, calculate when A will touch B. If A initially overlaps B, then it is <i>not</i> considered an intersection.
		/// </summary>
		public static bool IntersectMovingNormal(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact, out Vector2 normalA) {

			normalA = Vector2.Zero;

			// Relative velocity of B if A was stationary
			Vector2 v = velB - velA;

			firstTimeOfContact = 1f;

			if (v.X != 0) {

				if (v.X < 0f && b.Max.X <= a.Min.X + Epsilon) {
					// B is moving away from A to the left. no intersection
					return false;

				} else if (v.X > 0f && b.Min.X + Epsilon >= a.Max.X) {
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

						bool isDiag = v.Y != 0 && (IsEqual(b.Min.Y + projectionY, a.Max.Y) || IsEqual(b.Max.Y + projectionY, a.Min.Y));

						if (isDiag || b.Min.Y + projectionY < a.Max.Y && b.Max.Y + projectionY > a.Min.Y) {
							// y component of projection will overlap if B moves to time t

							firstTimeOfContact = t;

							normalA = v.X < 0 ? new Vector2(-1, 0) : new Vector2(1, 0);

							return true;

						}

					}

				}

			}

			if (v.Y != 0) {

				if (v.Y < 0f && b.Max.Y <= a.Min.Y + Epsilon) {
					// B is moving away from A downwards. no intersection
					return false;

				} else if (v.Y > 0f && b.Min.Y + Epsilon >= a.Max.Y) {
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

							firstTimeOfContact = t;

							normalA = v.Y < 0 ? new Vector2(0, -1) : new Vector2(0, 1);

							return true;
						}

					}

				}

			}

			return false;

		}

		

		#endregion

		#region Normals

		/// <summary>
		/// Get the normal of where 'of' touches 'against.'
		/// </summary>
		public static Vector2 GetNormal(BoxCollider of, BoxCollider against) {

			if (of.Position == against.Position) {
				// 'of' is exactly on top of 'against'. return upward normal
				return new Vector2(0, 1);
			}

			// positive insets means 'of' is inside 'against. negative means outside
			float top = against.Max.Y - of.Min.Y;
			float bottom = of.Max.Y - against.Min.Y;
			float left = of.Max.X - against.Min.X;
			float right = against.Max.X - of.Min.X;

			switch (new float[] { top, bottom, left, right }.IndexOfMin()) {
				case 0: // top
					return new Vector2(0, 1);
				case 1: // bottom
					return new Vector2(0, -1);
				case 2: // left
					return new Vector2(-1, 0);
				case 3: // right
					return new Vector2(1, 0);

				default:
					throw new Exception($"Failed to retrieve normal! (top {top}, bottom {bottom}, left {left}, right {right})");
			}

		}

		#endregion

		#region Raycasts (TODO)

		// Box p219

		#endregion

		#region Miscellaneous

		/// <summary>
		/// Returns true if A is sliding against B at a corner.
		/// </summary>
		public static bool IsSlidingCorner(BoxCollider a, BoxCollider b) {

			// Check horizontal
			if (IsEqual(a.Max.X, b.Min.X) || IsEqual(a.Min.X, b.Max.X)) {
				if (IsEqual(a.Min.Y, b.Max.Y) || IsEqual(a.Max.Y, b.Min.Y)) {
					return true;
				}

			}

			// Check vertical
			if (IsEqual(a.Max.Y, b.Min.Y) || IsEqual(a.Min.Y, b.Max.Y)) {
				if (IsEqual(a.Min.X, b.Max.X) || IsEqual(a.Max.X, b.Min.X)) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the solid should drag the actor during movement step.
		/// </summary>
		public static bool ShouldDrag(BoxCollider actor, BoxCollider solid, Vector2 actorVel, Vector2 solidVel) {

			if (actor.Min.Y < solid.Max.Y - Epsilon && actor.Max.Y > solid.Min.Y + Epsilon) {
				// Actor is touching solid in y-direction

				// Actor is left of solid and both are moving right
				if (actorVel.X > 0 && solidVel.X > 0 && actorVel.X >= solidVel.X && IsEqual(solid.Min.X, actor.Max.X)) return true;

				// Actor is right of solid and both are moving left
				if (actorVel.X < 0 && solidVel.X < 0 && actorVel.X <= solidVel.X && IsEqual(solid.Max.X, actor.Min.X)) return true;

			}

			if (actor.Min.X < solid.Max.X - Epsilon && actor.Max.X > solid.Min.X + Epsilon) {
				// Actor is touching solid in x-direction

				// Actor is below solid and both are moving up
				if (actorVel.Y > 0 && solidVel.Y > 0 && actorVel.Y >= solidVel.Y && IsEqual(solid.Min.Y, actor.Max.Y)) return true;

				// Actor is above solid and both are moving down
				if (actorVel.Y < 0 && solidVel.Y < 0 && actorVel.Y <= solidVel.Y && IsEqual(solid.Max.Y, actor.Min.Y)) return true;

			}

			return false;

		}

		/// <summary>
		/// Calculate the closest point where A is outside of B. Do not use if A is overlapping/outside B.
		/// </summary>
		public static Vector2 Depenetrate(BoxCollider a, BoxCollider b) {

			if (a.Position == b.Position) {
				// Objects are exactly on top of each other. Push A below B.
				return b.Position + new Vector2(0, -b.Extents.Y - a.Extents.Y);
			}

			float dx = 0f;
			float dy = 0f;

			if (a.Position.X > b.Position.X) dx = b.Max.X - a.Min.X;
			if (a.Position.X < b.Position.X) dx = b.Min.X - a.Max.X;
			if (a.Position.X == b.Position.X) dx = float.MaxValue;

			if (a.Position.Y > b.Position.Y) dy = b.Max.Y - a.Min.Y;
			if (a.Position.Y < b.Position.Y) dy = b.Min.Y - a.Max.Y;
			if (a.Position.Y == b.Position.Y) dy = float.MaxValue;

			if (Math.Abs(dx) < Math.Abs(dy)) {
				return a.Position + new Vector2(dx, 0);
			} else {
				return a.Position + new Vector2(0, dy);
			}
		}

		#endregion

		#region Comparison Operators with Custom Epsilon

		/// <summary>
		/// A == B by tolerance Epsilon.
		/// </summary>
		public static bool IsEqual(float a, float b) {
			return Math.Abs(a - b) < Epsilon;
		}

		/// <summary>
		/// Lower ~&lt; Value ~&lt; Upper.
		/// </summary>
		public static bool IsWithinLenient(float lower, float value, float upper) {
			return lower <= value + Epsilon && value <= upper + Epsilon;
		}

		/// <summary>
		/// Value &lt;&lt; Value &lt;&lt; Upper.
		/// </summary>
		public static bool IsWithinStrict(float lower, float value, float upper) {
			return lower < value - Epsilon && value < upper - Epsilon;
		}

		/// <summary>
		/// Value in [Lower, Upper]. This function is exact.
		/// </summary>
		public static bool IsWithin(float lower, float value, float upper) {
			return lower <= value && value <= upper;
		}

		#endregion

	}

}