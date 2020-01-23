using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class OffsetSpan : UpdateSpan {

		public Vector2 Offset { get; set; }

		public OffsetSpan(Vector2 offset) : this(0, 0, offset) { }

		public OffsetSpan(int startIndex, int length, Vector2 offset) : 
			base(startIndex, length) {
			Offset = offset;
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		internal override void Update(Typograph typograph, GlyphSequence glyphs) {
			foreach (var glyph in glyphs) {
				glyph.Offset += Offset;
			}
		}

		public override Span CloneSpan() => new OffsetSpan(StartIndex, Length, Offset);

		protected override string BaseToString() => $"Offset '{Offset}'";

	}

}