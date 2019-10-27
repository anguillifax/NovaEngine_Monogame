using Microsoft.Xna.Framework;
using System;

namespace Nova.PhysicsEngine {

	public static class VelocityCombinationMath {

		/// <summary>
		/// Applies 'applyOver' over 'initial' if going same direction. Otherwise returns inital unchanged.
		/// </summary>
		public static float Over(float initial, float applyOver) {
			if (initial * applyOver >= 0) {
				// going same direction
				return Math.Abs(initial) > Math.Abs(applyOver) ? initial : applyOver;

			} else {
				// going different directions
				return initial;
			}

		}

		public static Vector2 Over(Vector2 initial, Vector2 applyOver) {
			return new Vector2(Over(initial.X, applyOver.X), Over(initial.Y, applyOver.Y));
		}

	}

}