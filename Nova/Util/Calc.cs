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

	}

}