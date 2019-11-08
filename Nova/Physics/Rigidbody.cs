using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public abstract class Rigidbody : Component {

		/// <summary>
		/// Represents the desired movement during the next physics update
		/// </summary>
		public Vector2 Velocity;

		public readonly List<BoxCollider> Colliders;
		
		/// <summary>
		/// For debug only
		/// </summary>
		public BoxCollider MainCollider => Colliders[0];

		public readonly List<ContactPoint> Contacts = new List<ContactPoint>();

		internal ProcessingData ProcessingData = new ProcessingData();

		public int Layer { get; set; }



		public Rigidbody(Entity parent, params BoxCollider[] colliders) : base(parent) {
			Colliders = new List<BoxCollider>(colliders);
			foreach (var item in Colliders) {
				item.Rigidbody = this;
			}
			Physics.AllRigidbodies.Add(this);
		}

		internal void SetContacts(IEnumerable<ContactPoint> contacts) {
			Contacts.ClearAdd(contacts);
		}

	}

}