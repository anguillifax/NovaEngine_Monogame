using Microsoft.Xna.Framework;
using Nova.Linq;
using System;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Defines the mathematics behind all possible collisions between different types of colliders.
	/// Adapted from Christer Ericson's Real-Time Collision Book.
	/// </summary>
	[Obsolete]
	public static class PhysicsMathExact {

		public const float Epsilon = 1e-5f;

		#region Overlaps

		/// <summary>
		/// Returns true if A is very nearly overlapping B.
		/// </summary>
		public static bool IsOverlapping(BoxCollider a, BoxCollider b) {
			if (Math.Abs(a.Position.X - b.Position.X) - Epsilon > a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) - Epsilon > a.Extents.Y + b.Extents.Y) return false;
			return true;
		}

		/// <summary>
		/// Tests if A is inside of B. If A and B are aligned on the edges, this function returns false.
		/// </summary>
		public static bool IsInside(BoxCollider a, BoxCollider b) {
			if (Math.Abs(a.Position.X - b.Position.X) + Epsilon > a.Extents.X + b.Extents.X) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) + Epsilon > a.Extents.Y + b.Extents.Y) return false;
			return true;
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

		/// <summary>
		/// Get the normal of where 'of' touches 'against.'
		/// </summary>
		public static Vector2 GetNormal(BoxCollider of, BoxCollider against) {

			if (of.Position == against.Position) {
				return new Vector2(0, 1);
			}

			// find closest separation distance and return normal in that direction

			var insets = new float[] {
					against.Max.Y - of.Min.Y, // top
					against.Min.Y - of.Max.Y, // bottom
					against.Min.X - of.Max.X, // left
					against.Max.X - of.Min.X // right
				};

			int idx = insets.IndexOfMin();

			Console.WriteLine("Smallest is at index {0} of value {1}", idx, insets[idx]);

			switch (insets.IndexOfMin()) {
				case 0: // top
					return new Vector2(0, 1);
				case 1: // bottom
					return new Vector2(0, -1);
				case 2: // left
					return new Vector2(-1, 0);
				case 3: // right
					return new Vector2(1, 0);

				default:
					throw new Exception($"Failed to retrieve normal! Insets {insets.ToPrettyString()}");
			}

		}

		public static Vector2 GetNormal(Vector2 point, BoxCollider box) {

			Vector2 normal = Vector2.Zero;

			if (point.X < box.Position.X) normal.X = -1;
			if (point.X > box.Position.X) normal.X = 1;

			if (point.Y < box.Position.Y) normal.Y = -1;
			if (point.Y > box.Position.Y) normal.Y = 1;

			return normal.LengthSquared() == 0 ? Vector2.UnitX : normal.GetNormalized();
		}

		#endregion

		#region Moving Objects

		/// <summary>
		/// Returns time when two moving boxes overlap. If A overlaps B, then it is considered an intersection.
		/// </summary>
		public static bool IntersectMoving(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {
			if (IsOverlapping(a, b)) {
				firstTimeOfContact = 0f;
				return true;
			} else {
				return IntersectMovingNoOverlap(a, b, velA, velB, out firstTimeOfContact);
			}
		}

		/// <summary>
		/// Returns time when two moving boxes overlap. If A overlaps B, then it is NOT considered an intersection.
		/// </summary>
		public static bool IntersectMovingNoOverlap(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {

			// If A is stationary, v is relative velocity of B
			Vector2 v = velB - velA;

			firstTimeOfContact = 1f;
			bool hit = false;

			if (v.X != 0) {

				if (v.X < 0f && b.Max.X <= a.Min.X) {
					// B is moving away from A to the left. no intersection
					return false;

				} else if (v.X > 0f && b.Min.X >= a.Max.X) {
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

				if (v.Y < 0f && b.Max.Y <= a.Min.Y) {
					// B is moving away from A downwards. no intersection
					return false;

				} else if (v.Y > 0f && b.Min.Y >= a.Max.Y) {
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
		/// Returns time when two moving boxes overlap. If A overlaps B, then it is NOT considered an intersection.
		/// </summary>
		public static bool IntersectMovingNoOverlapFuzzy(BoxCollider a, BoxCollider b, Vector2 velA, Vector2 velB, out float firstTimeOfContact) {

			// If A is stationary, v is relative velocity of B
			Vector2 v = velB - velA;

			firstTimeOfContact = 1f;
			bool hit = false;

			if (v.X != 0) {

				if (v.X < 0f && IsLessLenient(b.Max.X, a.Min.X)) {
					// B is moving away from A to the left. no intersection
					Console.WriteLine("{0} e x <", a);
					return false;

				} else if (v.X > 0f && IsGreaterLenient(b.Min.X, a.Max.X)) {
					// B is moving away from A to the right. no intersection
					Console.WriteLine("{0} e x >", a);
					return false;

				} else {
					// B is approaching A on x axis. possible intersection

					float t;
					if (v.X > 0f) {
						t = (a.Min.X - b.Max.X) / v.X;
					} else {
						t = (a.Max.X - b.Min.X) / v.X;
					}

					Console.WriteLine("{0} x {1}", a, t);

					if (IsWithinLenient(0f, t, 1f)) {

						float projectionY = v.Y * t;

						if (IsLessStrict(b.Min.Y + projectionY, a.Max.Y) && IsGreaterStrict(b.Max.Y + projectionY, a.Min.Y)) {
							// y component of projection will overlap if B moves to time t
							firstTimeOfContact = Math.Min(t, firstTimeOfContact);
							hit = true;
						}

					}

				}

			}

			if (v.Y != 0) {

				if (v.Y < 0f && IsLessLenient(b.Max.Y, a.Min.Y)) {
					// B is moving away from A downwards. no intersection
					Console.WriteLine("{0} e y <", a);
					return false;

				} else if (v.Y > 0f && IsGreaterLenient(b.Min.Y, a.Max.Y)) {
					// B is moving away from A upwards. no intersection
					Console.WriteLine("{0} e y >", a);
					return false;

				} else {
					// B is approaching A on y axis. possible intersection

					float t;
					if (v.Y > 0f) {
						t = (a.Min.Y - b.Max.Y) / v.Y;
					} else {
						t = (a.Max.Y - b.Min.Y) / v.Y;
					}

					Console.WriteLine("{0} y {1}", a, t);

					if (IsWithinLenient(0f, t, 1f)) {

						float projectionX = v.X * t;

						if (IsLessStrict(a.Min.X, b.Max.X + projectionX) && IsGreaterStrict(a.Max.X, b.Min.X + projectionX)) {
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
		public static bool IntersectPush(BoxCollider solid, BoxCollider actor, Vector2 solidVel, out Vector2 actorMoveDelta) {

			actorMoveDelta = Vector2.Zero;

			// diagonals are considered a horizontal push

			if (solidVel.X != 0) {

				if (solidVel.X < 0f && solid.Max.X <= actor.Min.X) {
					// solid is moving away from actor to the left. no intersection
					return false;

				} else if (solidVel.X > 0f && solid.Min.X >= actor.Max.X) {
					// solid is moving away from actor to the right. no intersection
					return false;

				} else {
					// solid is approaching/touching actor on x axis. possible intersection

					float timeOfContact;
					if (solidVel.X > 0f) {
						timeOfContact = (actor.Min.X - solid.Max.X) / solidVel.X;
					} else {
						timeOfContact = (actor.Max.X - solid.Min.X) / solidVel.X;
					}

					if (0f <= timeOfContact && timeOfContact <= 1f) {

						float projectionY = solidVel.Y * timeOfContact;

						if (solid.Min.Y + projectionY <= actor.Max.Y && solid.Max.Y + projectionY >= actor.Min.Y) {
							// vertical edge of solid will be touching actor at time t

							if (solidVel.Y == 0) {
								// pushing horizontally only. push actor remaining distance after contact
								actorMoveDelta = new Vector2(solidVel.X * (1 - timeOfContact), 0);
								return true;

							} else {
								float timeOfSeparation;
								if (solidVel.Y > 0) {
									timeOfSeparation = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
								} else {
									timeOfSeparation = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
								}

								actorMoveDelta = new Vector2(solidVel.X * (Math.Min(1, timeOfSeparation) - timeOfContact), 0);
								return true;
							}

						}

					}

				}

			}

			if (solidVel.Y != 0) {

				if (solidVel.Y < 0f && solid.Max.Y <= actor.Min.Y) {
					// solid is moving away from actor downward. no intersection
					return false;

				} else if (solidVel.Y > 0f && solid.Min.Y >= actor.Max.Y) {
					// solid is moving away from actor upward. no intersection
					return false;

				} else {
					// solid is approaching/touching actor on y axis. possible intersection

					float timeOfContact;
					if (solidVel.Y > 0f) {
						timeOfContact = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
					} else {
						timeOfContact = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
					}

					if (0f <= timeOfContact && timeOfContact <= 1f) {

						float projectionX = solidVel.X * timeOfContact;

						if (solid.Min.X + projectionX <= actor.Max.X && solid.Max.X + projectionX >= actor.Min.X) {
							// horizontal edge of solid will be touching actor at time t

							if (solidVel.X == 0) {
								// pushing vertically only. push actor remaining distance after contact
								actorMoveDelta = new Vector2(0, solidVel.Y * (1 - timeOfContact));
								return true;

							} else {
								float timeOfSeparation;
								if (solidVel.X > 0) {
									timeOfSeparation = (actor.Max.X - solid.Min.X) / solidVel.X;
								} else {
									timeOfSeparation = (actor.Min.X - solid.Max.X) / solidVel.X;
								}

								actorMoveDelta = new Vector2(0, solidVel.Y * (Math.Min(1, timeOfSeparation) - timeOfContact));
								return true;
							}

						}

					}

				}

			}


			return false;

		}

		/// <summary>
		/// Given stationary actor and moving solid, calculate how far the solid pushes the actor.
		/// </summary>
		public static bool IntersectPushFuzzy(BoxCollider solid, BoxCollider actor, Vector2 solidVel, out Vector2 actorMoveDelta) {

			actorMoveDelta = Vector2.Zero;

			// diagonals are considered a horizontal push

			if (solidVel.X != 0) {

				if (solidVel.X < 0f && IsLessStrict(solid.Max.X, actor.Min.X)) {
					// solid is moving away from actor to the left. no intersection
					return false;

				} else if (solidVel.X > 0f && IsGreaterStrict(solid.Min.X, actor.Max.X)) {
					// solid is moving away from actor to the right. no intersection
					return false;

				} else {
					// solid is approaching/touching actor on x axis. possible intersection

					float timeOfContact;
					if (solidVel.X > 0f) {
						timeOfContact = (actor.Min.X - solid.Max.X) / solidVel.X;
					} else {
						timeOfContact = (actor.Max.X - solid.Min.X) / solidVel.X;
					}

					if (IsWithinLenient(0f, timeOfContact, 1f)) {

						float projectionY = solidVel.Y * timeOfContact;

						if (IsLessStrict(solid.Min.Y + projectionY, actor.Max.Y) && IsGreaterStrict(solid.Max.Y + projectionY, actor.Min.Y)) {
							// vertical edge of solid will be touching actor at time t

							if (solidVel.Y == 0) {
								// pushing horizontally only. push actor remaining distance after contact
								actorMoveDelta = new Vector2(solidVel.X * (1 - timeOfContact), 0);
								return true;

							} else {
								float timeOfSeparation;
								if (solidVel.Y > 0) {
									timeOfSeparation = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
								} else {
									timeOfSeparation = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
								}

								actorMoveDelta = new Vector2(solidVel.X * (Math.Min(1, timeOfSeparation) - timeOfContact), 0);

								return true;
							}

						}

					}

				}

			}

			if (solidVel.Y != 0) {

				if (solidVel.Y < 0f && IsLessStrict(solid.Max.Y, actor.Min.Y)) {
					// solid is moving away from actor downward. no intersection
					return false;

				} else if (solidVel.Y > 0f && IsGreaterStrict(solid.Min.Y, actor.Max.Y)) {
					// solid is moving away from actor upward. no intersection
					return false;

				} else {
					// solid is approaching/touching actor on y axis. possible intersection

					float timeOfContact;
					if (solidVel.Y > 0f) {
						timeOfContact = (actor.Min.Y - solid.Max.Y) / solidVel.Y;
					} else {
						timeOfContact = (actor.Max.Y - solid.Min.Y) / solidVel.Y;
					}

					if (IsWithinLenient(0f, timeOfContact, 1f)) {

						float projectionX = solidVel.X * timeOfContact;

						if (IsLessStrict(solid.Min.X + projectionX, actor.Max.X) && IsGreaterStrict(solid.Max.X + projectionX, actor.Min.X)) {
							// horizontal edge of solid will be touching actor at time t

							if (solidVel.X == 0) {
								// pushing vertically only. push actor remaining distance after contact
								actorMoveDelta = new Vector2(0, solidVel.Y * (1 - timeOfContact));
								return true;

							} else {
								float timeOfSeparation;
								if (solidVel.X > 0) {
									timeOfSeparation = (actor.Max.X - solid.Min.X) / solidVel.X;
								} else {
									timeOfSeparation = (actor.Min.X - solid.Max.X) / solidVel.X;
								}

								actorMoveDelta = new Vector2(0, solidVel.Y * (Math.Min(1, timeOfSeparation) - timeOfContact));
								return true;
							}

						}

					}

				}

			}


			return false;

		}

		#endregion

		#region Overlap Resolution (Depenetration)

		/// <summary>
		/// Calculate the closest point where A is outside of B. Do not use if A is overlapping/outside B.
		/// </summary>
		public static Vector2 Depenetrate(BoxCollider a, BoxCollider b) {

			if (a.Position == b.Position) {
				// Objects are exactly on top of each other. Push A above B.
				return b.Position + new Vector2(0, b.Extents.Y + a.Extents.Y);
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

		#region Raycasts (TODO)

		// Box p219

		#endregion

		#region Specialized

		/// <summary>
		/// If vel moves into plane defined by normal, gives point on plane instead. Otherwise returns vel unchanged.
		/// </summary>
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

		//public static Vector2 GetAllowedVelocity(Vector2 vel, BoxCollider of, BoxCollider against) {

		//	// check diagonals

		//	if (IsEqual(of.Max.X, against.Min.X)) {
		//		if (IsEqual(of.Min.Y, against.Max.Y)) {
		//			// top left

		//		}
		//		if (IsEqual(of.Max.Y, against.Min.Y)) {
		//			// bottom left
		//		}
		//	}
		//	if (IsEqual(of.Min.X, against.Max.X)) {
		//		if (IsEqual(of.Min.Y, against.Max.Y)) {
		//			// top right

		//		}
		//		if (IsEqual(of.Max.Y, against.Min.Y)) {
		//			// top left

		//		}
		//	}

		//}

		/// <summary>
		/// Returns true if this collider should not be considered for interaction.
		/// </summary>
		public static bool CheckSkipSlideTowardsBox(Vector2 vel, BoxCollider of, BoxCollider against) {
			if (vel.X < 0 && of.Min.X < against.Max.X + Epsilon) return true;
			if (vel.X > 0 && of.Max.X > against.Max.X - Epsilon) return true;
			if (vel.Y < 0 && of.Max.Y < against.Min.Y + Epsilon) return true;
			if (vel.Y > 0 && of.Min.Y > against.Max.Y + Epsilon) return true;
			return false;
		}

		#endregion

		/// <summary>
		/// Returns the squared value of v. Created exclusively to help improve mathematical
		/// clarity within this class.
		/// </summary>
		private static float Squared(float v) {
			return v * v;
		}

		#region Comparison Operators with Custom Epsilon

		/// <summary>
		/// a equals b by tolerance epsilon
		/// </summary>
		public static bool IsEqual(float a, float b) {
			return Math.Abs(a - b) < Epsilon;
		}

		/// <summary>
		/// a must be significantly less than b
		/// </summary>
		public static bool IsLessStrict(float a, float b) {
			return a < b - Epsilon;
		}

		/// <summary>
		/// a can be slightly more than b
		/// </summary>
		public static bool IsLessLenient(float a, float b) {
			return a < b + Epsilon;
		}

		/// <summary>
		/// a must be significantly more than b
		/// </summary>
		public static bool IsGreaterStrict(float a, float b) {
			return a > b + Epsilon;
		}

		/// <summary>
		/// a can be slightly less than b
		/// </summary>
		public static bool IsGreaterLenient(float a, float b) {
			return a > b - Epsilon;
		}

		/// <summary>
		/// value can be slightly above or below bounds
		/// </summary>
		public static bool IsWithinLenient(float lower, float value, float upper) {
			return IsLessLenient(lower, value) && IsLessLenient(value, upper);
		}

		/// <summary>
		/// value has to be within bounds by significant amount
		/// </summary>
		public static bool IsWithinStrict(float lower, float value, float upper) {
			return IsLessLenient(lower, value) && IsLessLenient(value, upper);
		}

		#endregion

	}

}