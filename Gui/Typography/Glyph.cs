using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nova.Gui.Typography {

	public sealed class Glyph {

		private GlyphData Data { get; }

		/// <summary>
		/// The character this glyph represents.
		/// </summary>
		public char Character => Data.Character;

		/// <summary>
		/// The parent font.
		/// </summary>
		public Font Font { get; }

		/// <summary>
		/// Position of character in typograph. Changes to this value are permanent.
		/// </summary>
		public Vector2 Position { get; set; }

		/// <summary>
		/// Current offset from character's normal position. This value is always set to (0, 0) before spans are applied.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// The color of the glyph.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Get the width of the glyph.
		/// </summary>
		public int CharacterWidth => Data.DrawRect.Width;

		/// <summary>
		/// Get the distance to the next glyph.
		/// </summary>
		public int XAdvance => Data.XAdvance;

		internal Glyph(Font font, GlyphData glyphData) {
			Font = font;
			Data = glyphData;
		}

		internal void Render() {
			MDraw.SpriteBatch.Draw(Data.TextureSheet, Position + Offset + Data.Offset.ToVector2(), Data.DrawRect,
				Color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
		}

		public override string ToString() => $"Glyph ('{TextUtil.GetRepresentation(Character)}' {Position + Offset})";

	}

}