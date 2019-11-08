using Microsoft.Xna.Framework;

namespace Nova.PhysicsEngine {

	internal class DefaultVelocityCombinationRuleset : IVelocityCombinationRuleset {

		public VelocityPair Get(Rigidbody left, Rigidbody right) {

			Vector2 leftVel = left.ProcessingData.CalcVel;
			Vector2 rightVel = right.ProcessingData.CalcVel;

			//solid vs solid
			if (left is SolidRigidbody && right is SolidRigidbody) {
				return VelocityMath.CollideSlide(left.MainCollider, right.MainCollider, leftVel, rightVel, 0.5f, 0.5f);
			}

			// actor vs actor
			if (left is ActorRigidbody && right is ActorRigidbody) {
				return VelocityMath.CollideSlide(left.MainCollider, right.MainCollider, leftVel, rightVel, 0.5f, 0.5f);
			}

			return new VelocityPair(leftVel, rightVel);

		}

	}

}