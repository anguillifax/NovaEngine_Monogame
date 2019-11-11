using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Defines the resultant velocity of A and B when in contact.
	/// </summary>
	public interface IVelocityCombinationRuleset {
		VelocityPair Get(Rigidbody a, Rigidbody b);
	}

}