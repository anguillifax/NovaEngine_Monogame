using Microsoft.Xna.Framework;

namespace Nova.PhysicsEngine {

	public struct VelocityPair {

		public Vector2 left, right;

		public VelocityPair(Vector2 left, Vector2 right) {
			this.left = left;
			this.right = right;
		}

		public VelocityPair(VelocityPair other) {
			left = other.left;
			right = other.right;
		}

	}

}