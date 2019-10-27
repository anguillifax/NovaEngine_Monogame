using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.PhysicsEngine {

	public class SolidRigidbody : Rigidbody {

		public SolidRigidbody(Entity parent, params BoxCollider[] colliders) :
			base(parent, colliders) {

			Physics.AllSolids.Add(this);
		}

	}

}