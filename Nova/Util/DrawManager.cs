using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public static class MDraw {

		public static SpriteBatch SpriteBatch { get; private set; }

		public static void Initialize(GraphicsDevice device) {
			SpriteBatch = new SpriteBatch(device);
		}

		public static void Begin() {
			SpriteBatch.Begin(SpriteSortMode.Deferred);
		}

		public static void End() {
			SpriteBatch.End();
		}

		public static void Draw(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			Console.WriteLine("{0} at {1} scale {2}", texture.Name, position, scale);
			SpriteBatch.Draw(texture, Camera.Position + position, null, Color.White, rotation, origin, Camera.Scale * scale, SpriteEffects.None, 0);
		}

		public static void Write(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(Engine.DefaultFont, text, pos, color);
		}

	}

}