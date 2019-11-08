using Microsoft.Xna.Framework;
using System;

namespace Nova.PhysicsEngine {

	public static class VelocityMath {

		/// <summary>
		/// If vel moves into plane defined by normal, gives point on plane instead. Otherwise returns vel unchanged.
		/// </summary>
		public static Vector2 IntoPlane(Vector2 vel, Vector2 normal) {

			var dot = Vector2.Dot(vel, normal);

			if (dot < 0) {
				// point is below plane. snap to plane
				return vel - dot * normal;
			} else {
				return vel;
			}

		}

		public static float WeightedAverage(float left, float right, float leftWeight) {
			return leftWeight * left + (1 - leftWeight) * right;
		}

		/// <summary>
		/// Returns true if object located at posLeft is moving toward object located at posRight.
		/// <para>Returns false if both objects stationary, or both objects are at same position</para>
		/// </summary>
		public static bool IsMovingTowardEachOther(float posA, float posB, float velA, float velB) {

			if (velA == 0 && velB == 0) return false;
			if (posA == posB) return false;

			if (posA < posB) {

				if (velA == 0) {
					return velB < 0;

				} else if (velB == 0) {
					return velA > 0;

				} else {
					return velA > 0 && velB < 0;
				}

			}

			if (posA > posB) {

				if (velA == 0) {
					return velB > 0;

				} else if (velB == 0) {
					return velA < 0;

				} else {
					return velB > 0 && velA < 0;
				}

			}

			return false;

		}

		/// <summary>
		/// If objects are moving toward each other in x direction, returns weighted average of x velocities and original y velocities.
		/// <para>Repeats logic in y direction.</para>
		/// </summary>
		public static VelocityPair CollideSlide(BoxCollider left, BoxCollider right, Vector2 leftVel, Vector2 rightVel, float horzLeftWeight = 0.5f, float vertLeftWeight = 0.5f) {

			if (PhysicsMath.GetNormal(left, right).X != 0) {
				// Horizontal collision

				if (IsMovingTowardEachOther(left.Position.X, right.Position.X, leftVel.X, rightVel.X)) {

					// Objects are moving toward each other in x direction, return weighted average of x velocities.
					float xAvg = WeightedAverage(leftVel.X, rightVel.X, horzLeftWeight);
					return new VelocityPair(new Vector2(xAvg, leftVel.Y), new Vector2(xAvg, rightVel.Y));

				} else {

					// Objects are moving apart, return original velocities.
					return new VelocityPair(leftVel, rightVel);

				}


			} else {
				// Vertical collision

				if (IsMovingTowardEachOther(left.Position.Y, right.Position.Y, leftVel.Y, rightVel.Y)) {

					// Objects are moving toward each other in y direction, return weighted average of y velocities.
					float yAvg = WeightedAverage(leftVel.Y, rightVel.Y, vertLeftWeight);
					return new VelocityPair(new Vector2(leftVel.X, yAvg), new Vector2(rightVel.X, yAvg));

				} else {

					// Objects are moving apart, return original velocities.
					return new VelocityPair(leftVel, rightVel);

				}

			}

		}

	}

}