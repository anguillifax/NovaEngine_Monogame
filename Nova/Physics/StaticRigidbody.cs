namespace Nova.PhysicsEngine {

	public class StaticRigidbody : Rigidbody {

		public StaticRigidbody(Entity parent, BoxCollider collider) :
			base(parent, collider) {

			Physics.AllStatics.Add(this);
		}

	}

}