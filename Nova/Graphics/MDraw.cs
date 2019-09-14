using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public static class MDraw {

		public static SpriteBatch SpriteBatch { get; private set; }
		public static SpriteFont DefaultFont { get; private set; }
		public static Camera Camera { get; private set; }

		public static void Initialize() {
			SpriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
			Camera = new Camera(Screen.Center);
		}

		public static void LoadContent() {
			DefaultFont = Engine.Instance.Content.Load<SpriteFont>("Font1");
		}

		public static void Begin() {
			SpriteBatch.Begin(SpriteSortMode.Deferred);
		}

		public static void End() {
			SpriteBatch.End();
		}

		public static void Draw(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, Camera.PositionToGlobal(position), null, Color.White, rotation, origin, Camera.ScaleToGlobal(scale), SpriteEffects.None, 0);
		}

		public static void DrawGlobal(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, position, null, Color.White, rotation, scale, origin, SpriteEffects.None, 0);
		}

		public static void Write(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, Camera.PositionToGlobal(pos), color, 0f, Vector2.Zero, Camera.ScaleToGlobal(Vector2.One), SpriteEffects.None, 0f);
		}

		public static void WriteGlobal(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, pos, color);
		}

	}

}