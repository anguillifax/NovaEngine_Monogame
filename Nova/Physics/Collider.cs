using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public interface ICollider {

		void Draw();

	}

	public class BoxCollider : Component, ICollider {

		public Vector2 Origin { get; set; }
		public Vector2 Size { get; set; }

		public BoxCollider(Entity parent, Vector2 origin, Vector2 size) :
			base(parent) {
			Origin = origin;
			Size = size;
		}

		public void Draw() {
			MDraw.Begin();
			MDraw.DrawBox(Entity.Position + Origin, Size / 2, Physics.ColliderDrawColor);
			MDraw.End();
		}
	}

}