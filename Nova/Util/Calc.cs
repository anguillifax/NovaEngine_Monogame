using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Nova {

	public static class Calc {

		/// <summary>
		/// Gets the max of A and B based on magnitude. If |A| == |B|, returns A.
		/// </summary>
		public static float AbsMax(float a, float b) {
			return Math.Abs(a) >= Math.Abs(b) ? a : b;
		}

		/// <summary>
		/// Gets the min of A and B based on magnitude. If |A| == |B|, returns A.
		/// </summary>
		public static float AbsMin(float a, float b) {
			return Math.Abs(a) <= Math.Abs(b) ? a : b;
		}

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
		/// Rotate a vector around the origin. Clockwise is positive.
		/// </summary>
		public static Vector2 RotateRadians(Vector2 vec, float radians) {
			float cos = (float)Math.Cos(-radians);
			float sin = (float)Math.Sin(-radians);
			return new Vector2(
				vec.X * cos - vec.Y * sin,
				vec.X * cos + vec.Y * sin
			);
		}

		/// <summary>
		/// Rotate a vector around the origin. Clockwise is positive.
		/// </summary>
		public static Vector2 RotateDegrees(Vector2 vec, float degrees) {
			return RotateRadians(vec, MathHelper.ToRadians(degrees));
		}

		/// <summary>
		/// Returns a Vector2 on the perimeter of a circle. Angle is in radians from top with clockwise as positive.
		/// </summary>
		public static Vector2 ProjectPoint(float angle, float distance) {
			return new Vector2(distance * (float)Math.Sin(angle), distance * (float)Math.Cos(angle));
		}

		public static float Round(float value, float boundary) {
			return (float)Math.Round(value / boundary) * boundary;
		}

		public static Vector2 Round(Vector2 value, float boundary) {
			return new Vector2(Round(value.X, boundary), Round(value.Y, boundary));
		}

		public static int RoundToInt(float value) {
			return (int)Math.Round(value);
		}

		public static Vector2 Round(Vector2 value) {
			return new Vector2(RoundToInt(value.X), RoundToInt(value.Y));
		}

	}

}