using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	public sealed class GlyphData {

		public char Character { get; }
		public Texture2D TextureSheet { get; }
		public Rectangle DrawRect { get; }
		public Point Offset { get; }
		public int XAdvance { get; }

		public GlyphData(char character, Texture2D textureSheet, Rectangle drawRect, Point offset, int xAdvance) {
			Character = character;
			TextureSheet = textureSheet;
			DrawRect = drawRect;
			Offset = offset;
			XAdvance = xAdvance;
		}
	}

}