using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Nova {

	public class SpriteRenderer : IRenderer {

		public Entity Entity { get; set; }
		public Texture2D Texture { get; set; }

		public Vector2 Origin { get; set; }

		public SpriteRenderer(Entity parent, Texture2D texture) {
			Entity = parent ?? throw new ArgumentNullException();
			Texture = texture;
			Origin = new Vector2();
			if (texture != null) {
				Origin = new Vector2(texture.Width / 2, texture.Height / 2);
			}
		}

		public void Render() {
			if (Texture == null) return;
			MDraw.Begin();
			MDraw.Draw(Texture, Entity.Position, Entity.Rotation, Origin, Entity.Scale);
			MDraw.End();
		}

	}

}