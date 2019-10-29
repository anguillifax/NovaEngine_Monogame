using Microsoft.Xna.Framework;

namespace Nova.PhysicsEngine {

	internal class DefaultVelocityCombinationRuleset : IVelocityCombinationRuleset {

		public VelocityPair Get(Rigidbody left, Rigidbody right) {

			Vector2 leftVel = left.ProcessingData.CalcVel;
			Vector2 rightVel = right.ProcessingData.CalcVel;

			// solid vs solid
			//if (left is SolidRigidbody && right is SolidRigidbody) {
			//	return VelocityPair.Average(leftVel, rightVel);
			//}

			// solid vs actor
			//if (left is SolidRigidbody && right is ActorRigidbody) {
			//	return new VelocityPair(leftVel, VelocityCombinationMath.Over(rightVel, leftVel));
			//}
			//if (left is ActorRigidbody && right is SolidRigidbody) {
			//	return new VelocityPair(VelocityCombinationMath.Over(leftVel, rightVel), rightVel);
			//}

			// actor vs actor
			if (left is ActorRigidbody && right is ActorRigidbody) {
				return VelocityPair.Average(leftVel, rightVel);
			}

			return new VelocityPair(leftVel, rightVel);

		}

	}

}