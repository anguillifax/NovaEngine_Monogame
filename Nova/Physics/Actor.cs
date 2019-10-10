using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public class Actor : Rigidbody {

		public Actor(Entity parent, params Collider[] colliders) :
			base(parent, colliders) {

			Physics.AllActors.Add(this);
		}

	}

}