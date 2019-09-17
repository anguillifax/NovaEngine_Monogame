using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class Rigidbody : Component {

		public Vector2 Velocity { get; set; }
		public BoxCollider Collider { get; set; }

		public bool ApplyGravity { get; set; }

		public Rigidbody(Entity parent) : base(parent) {
		}

	}

}