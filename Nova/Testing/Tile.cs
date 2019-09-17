using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Tiles {

	public class Tile : Entity {

		TileRenderer tileRenderer;

		public Tile(Scene scene, Texture2D tex, Vector2 pos) : 
			base(scene) {
			Position = pos;
			tileRenderer = new TileRenderer(this, tex);
		}

		public override void Draw() {
			tileRenderer.Render();
		}

	}

	public class TileRenderer : IRenderer {
		public Entity Entity { get; set; }
		public Texture2D Texture { get; set; }
		public Vector2 Origin { get; set; }

		public TileRenderer(Entity parent, Texture2D texture) {
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
			MDraw.Draw(Texture, Entity.Position, 0, Origin, Vector2.One);
			MDraw.End();
		}
	}
}