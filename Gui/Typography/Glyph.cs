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
		/// The size of the glyph.
		/// </summary>
		public float Size { get; set; }

		/// <summary>
		/// The factor that this glyph is larger or smaller than the unscaled glyph size.
		/// </summary>
		public float Factor => Size / Font.MaxSize;

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
		public float CharacterWidth => Data.DrawRect.Width * Factor;

		/// <summary>
		/// Get the distance to the next glyph.
		/// </summary>
		public float XAdvance => Data.XAdvance * Factor;

		/// <summary>
		/// Get the vertical spacing distance between lines.
		/// </summary>
		public float LineHeight => Font.LineHeight * Factor;

		internal Glyph(Font font, GlyphData glyphData) {
			Font = font;
			Data = glyphData;
		}

		internal void Render() {
			MDraw.SpriteBatch.Draw(Data.TextureSheet, Position + Offset + Factor * Data.Offset.ToVector2(), Data.DrawRect,
				Color, 0, Vector2.Zero, Factor, SpriteEffects.None, 0);
		}

		public override string ToString() => $"Glyph ('{TextUtil.GetRepresentation(Character)}' {Position + Offset})";

	}

}