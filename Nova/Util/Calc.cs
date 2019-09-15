using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Nova {

	public static class Calc {

		public static Vector2 ClampMagnitude(Vector2 vec, float max) {
			if (vec.LengthSquared() == 0) return vec;
			vec.Normalize();
			return vec * max;
		}

		public static void ClampMagnitude(ref Vector2 vec, float max) {
			if (vec.LengthSquared() == 0) return;
			vec.Normalize();
			vec *= max;
		}

		/// <summary>
		/// Clamp a value between [min, max]
		/// </summary>
		public static float Clamp(float value, float min, float max) {
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		/// <summary>
		/// Loops between [min, max]
		/// </summary>
		public static float Loop(float current, float min, float max) {
			if (min >= max) throw new ArgumentException("Min is not less than Max");

			float range = max - min;
			while (current > max) {
				current -= range;
			}
			while (current < min) {
				current += range;
			}
			return current;
		}

		/// <summary>
		/// Loops between [min, max)
		/// </summary>
		public static int Loop(int current, int min, int max) {
			if (min >= max) throw new ArgumentException("Min is not less than Max");

			int range = max - min;
			while (current >= max) {
				current -= range;
			}
			while (current < min) {
				current += range;
			}
			return current;
		}

		/// <summary>
		/// Returns a Vector2 on the perimeter of a circle. Angle is in radians from top with clockwise as positive.
		/// </summary>
		public static Vector2 ProjectPoint(float angle, float distance) {
			return new Vector2(distance * (float)Math.Sin(angle), distance * (float)Math.Cos(angle));
		}

	}

}