using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	/// <summary>
	/// GlyphData holds all common rendering data for a glyph. At any time, there is only one instance of a GlyphData for each character in a font.
	/// </summary>
	internal sealed class GlyphData {

		internal char Character { get; }
		internal Texture2D TextureSheet { get; }
		internal Rectangle DrawRect { get; }
		internal Point Offset { get; }
		internal int XAdvance { get; }

		internal GlyphData(char character, Texture2D textureSheet, Rectangle drawRect, Point offset, int xAdvance) {
			Character = character;
			TextureSheet = textureSheet;
			DrawRect = drawRect;
			Offset = offset;
			XAdvance = xAdvance;
		}
	}

}