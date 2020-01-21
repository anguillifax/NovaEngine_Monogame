using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nova.Gui.Typography {

	public sealed class Glyph {

		internal GlyphData Data { get; }

		/// <summary>
		/// Position of character in typograph. Changes to this value are permanent.
		/// </summary>
		public Vector2 CharacterPosition { get; set; }

		/// <summary>
		/// Current offset from character's normal position. This value is always set to (0, 0) before spans are applied.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// The color of the glyph.
		/// </summary>
		public Color Color { get; set; }

		internal Glyph(GlyphData glyphData) {
			Data = glyphData;
		}

		internal void Render(Vector2 typographTopLeft) {
			MDraw.SpriteBatch.Draw(Data.TextureSheet, typographTopLeft + CharacterPosition + Offset + Data.Offset.ToVector2(), Data.DrawRect,
				Color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
		}
	}

}