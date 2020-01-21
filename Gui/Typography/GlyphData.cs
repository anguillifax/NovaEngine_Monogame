using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

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