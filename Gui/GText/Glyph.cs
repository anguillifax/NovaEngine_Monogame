using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	public sealed class Glyph {

		internal GlyphData Data { get; }

		internal Vector2 Position { get; set; }
		internal Vector2 Offset { get; set; }
		internal Color Color { get; set; }

		internal Glyph(GlyphData glyphData) {
			Data = glyphData;
		}

		public void Render() {
			MDraw.SpriteBatch.Draw(Data.TextureSheet, Position + Offset + Data.Offset.ToVector2(), Data.DrawRect,
				Color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
		}
	}

}