using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project {

	public abstract class Component {

		public virtual void PreUpdate() {
		}

		public virtual void Update() {
		}

		public virtual void PostUpdate() {
		}

	}

}