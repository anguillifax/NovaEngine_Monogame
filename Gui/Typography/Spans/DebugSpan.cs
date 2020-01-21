using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class DebugSpan : UpdateSpan {

		public DebugSpan(int startIndex, int length) : base(startIndex, length) {
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		internal override void Update(Typograph typograph, GlyphSequence glyphs) {
			foreach (var glyph in glyphs) {
				glyph.Offset += new Vector2(0, -8f);
			}
		}

		protected override string BaseToString() => "Debug";

	}

}