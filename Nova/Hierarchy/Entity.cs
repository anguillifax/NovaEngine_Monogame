using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project {

	public abstract class Entity {

		public readonly List<Component> Components;

		protected Entity(bool active = true) {
			Active = active;
			Components = new List<Component>();
		}

		public bool Active { get; set; }

		public virtual void PreUpdate() {
			foreach (var item in Components) {
				item.PreUpdate();
			}
		}

		public virtual void Update() {
			foreach (var item in Components) {
				item.Update();
			}
		}

		public virtual void PostUpdate() {
			foreach (var item in Components) {
				item.PostUpdate();
			}
		}

	}

}