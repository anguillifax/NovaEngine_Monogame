using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public class ActorRigidbody : Rigidbody {

		public ActorRigidbody(Entity parent, params BoxCollider[] colliders) :
			base(parent, colliders) {

			Physics.AllActors.Add(this);
		}

	}

}