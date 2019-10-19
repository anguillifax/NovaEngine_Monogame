using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Defines the mathematics behind all possible collisions between different types of colliders.
	/// Adapted from Christer Ericson's Real-Time Collision Book.
	/// </summary>
	public static class PhysicsMath {

		/* Forms of comparison operators with custom Epsilon
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
		/// Tests if A is inside of B. If A and B are aligned on the edges, this function returns false.
		/// </summary>
		public static bool IsInside(BoxCollider a, BoxCollider b) {
			if (Math.Abs(a.Position.X - b.Position.X) >= a.Extents.X + b.Extents.X - Epsilon) return false;
			if (Math.Abs(a.Position.Y - b.Position.Y) >= a.Extents.Y + b.Extents.Y - Epsilon) return false;
			// separation distance must be much larger than extents.

			return true;
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

		#region Miscellaneous

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
			return lower < value - Epsilon && value < value - Epsilon;
		}

		#endregion

	}

}