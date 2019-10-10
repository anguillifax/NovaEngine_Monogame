using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Nova {

	public class SpriteRenderer : Component {

		public Texture2D Texture { get; set; }
		public Vector2 Offset { get; set; }
		public Vector2 Origin { get; set; }
		public int Depth { get; set; }

		public SpriteRenderer(Entity parent, Texture2D texture, int depth) :
			base(parent) {

			Texture = texture;
			Depth = depth;
			Offset = Vector2.Zero;
			Origin = Vector2.Zero;

			if (texture != null) {
				Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
			}
		}

		public override void Draw() {
			if (Texture == null) return;
			MDraw.Draw(Texture, Depth, Entity.Position + Offset, Entity.Rotation, Origin, Entity.Scale);
		}

	}

}