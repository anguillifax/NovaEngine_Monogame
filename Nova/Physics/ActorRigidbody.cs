using Microsoft.Xna.Framework;
using System;

namespace Nova.PhysicsEngine {

	public class ActorRigidbody : Rigidbody {

		public ActorRigidbody(Entity parent, params BoxCollider[] colliders) :
			base(parent, colliders) {

			Physics.AllActors.Add(this);
		}

		/// <summary>
		/// Return true if a solid should drag actor during its movement. Default behavior is to return false.
		/// </summary>
		public Predicate<SolidRigidbody> PredicateIsAttached { get; set; }

		/// <summary>
		/// Action to perform when crushed. Default action is to teleport to (0, 0).
		/// </summary>
		public Action ActionCrush { get; set; }

		/// <summary>
		/// Returns true if solid should drag actor during its movement.
		/// </summary>
		public bool IsAttached(SolidRigidbody solid) {
			return PredicateIsAttached == null ? false : PredicateIsAttached(solid);
		}

		/// <summary>
		/// Called when the actor is crushed between two solids.
		/// </summary>
		public void Crush() {
			if (ActionCrush == null) {
				Entity.Position = Vector2.Zero;
			} else {
				ActionCrush();
			}
		}

	}

}