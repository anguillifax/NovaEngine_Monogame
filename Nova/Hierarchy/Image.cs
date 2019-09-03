using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project {

	public class Image : EntityDrawable {

		public Texture2D texture;

		public Image(Texture2D texture) : base() {
			this.texture = texture;
			Origin = new Vector2(texture.Width / 2, texture.Height / 2);
		}

		public override void Draw() {
			DrawManager.Draw(texture, Position, Rotation, Origin, Scale);
			base.Draw();
		}

	}

}