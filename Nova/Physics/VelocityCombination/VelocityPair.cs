using Microsoft.Xna.Framework;

namespace Nova.PhysicsEngine {

	public struct VelocityPair {

		public Vector2 left, right;

		public VelocityPair(Vector2 left, Vector2 right) {
			this.left = left;
			this.right = right;
		}

		public static VelocityPair Average(Vector2 left, Vector2 right) {
			Vector2 avg = 0.5f * (left + right);
			return new VelocityPair(avg, avg);
		}

	}

}