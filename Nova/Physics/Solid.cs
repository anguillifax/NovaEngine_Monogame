using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.PhysicsEngine {

	public class Solid : Rigidbody {

		public Solid(Entity parent, params BoxCollider[] colliders) :
			base(parent, colliders) {

			Physics.AllSolids.Add(this);
		}

	}

}