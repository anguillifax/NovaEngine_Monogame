using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.PhysicsEngine;
using System;

namespace Nova {

	public class SpriteRenderer : Component {

		public Texture2D Texture { get; set; }
		public Vector2 LocalPosition { get; set; }
		public Vector2 TextureOrigin { get; set; }
		public int DrawDepth { get; set; }

		public SpriteRenderer(Entity parent, Texture2D texture, int depth) :
			base(parent) {

			if (parent == null) throw new ArgumentNullException("The parent entity cannot be null!");
			if (texture == null) throw new ArgumentNullException("The texture cannot be null!");

			Texture = texture;
			DrawDepth = depth;
			LocalPosition = Vector2.Zero;

			TextureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
		}

		public override void Draw() {
			MDraw.Draw(Texture, DrawDepth, Entity.Position + LocalPosition, Entity.Rotation, TextureOrigin, Entity.Scale);
		}

	}

}