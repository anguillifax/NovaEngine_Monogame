using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Project {

	public class AnimatedSprite {

		public Texture2D Texture { get; set; }
		public int Rows { get; set; }
		public int Columns { get; set; }
		private int currentFrame;
		private int TotalFrames { get { return Columns * Rows; } }

		public AnimatedSprite(Texture2D texture, int rows, int columns) {
			Texture = texture;
			Rows = rows;
			Columns = columns;
			currentFrame = 0;
		}

		public void Update() {
			++currentFrame;
			if (currentFrame >= TotalFrames) {
				currentFrame = 0;
			}
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position) {
			int width = Texture.Width / Columns;
			int height = Texture.Height / Rows;
			int row = (int)((float)currentFrame / Rows);
			int column = currentFrame % Columns;

			Rectangle sourceRect = new Rectangle(width * column, height * row, width, height);
			Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, width, height);

			spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
		}
	}

}