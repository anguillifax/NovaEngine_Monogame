using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public abstract class EntityDrawable : Entity {

		protected EntityDrawable(bool visible = true) {
			Visible = visible;
			Position = Vector2.Zero;
			Scale = Vector2.One;
			Tint = Color.White;
			Rotation = 0f;
		}

		public bool Visible { get; set; }

		public Vector2 Position;
		public float X {
			get {
				return Position.X;
			}
			set {
				Position.X = value;
			}
		}
		public float Y {
			get {
				return Position.Y;
			}
			set {
				Position.Y = value;
			}
		}

		public Vector2 Scale;
		public float Rotation;
		public Vector2 Origin;
		public Color Tint;



		public virtual void Draw() {
		}

	}

}