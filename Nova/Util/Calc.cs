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

	}

	public static class CustomExtensions {

		public static Vector2 Copy(this Vector2 vec) {
			return new Vector2(vec.X, vec.Y);
		}

		public static Vector3 Copy(this Vector3 vec) {
			return new Vector3(vec.X, vec.Y, vec.Z);
		}

		/// <summary>
		/// If dictionary contains key, existing value is updated to new value. If key is not in dictionary, it is added.
		/// </summary>
		public static void AddUpdate<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV value) {
			if (dict.ContainsKey(key)) {
				dict[key] = value;
			} else {
				dict.Add(key, value);
			}
		}

		/// <summary>
		/// If dictionary contains key, returns existing value. If dictionary does not contain key, returns parameter defaultValue.
		/// </summary>
		public static TV GetDefault<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV defaultValue) {
			if (dict.ContainsKey(key)) {
				return dict[key];
			} else {
				return defaultValue;
			}
		}

	}

}