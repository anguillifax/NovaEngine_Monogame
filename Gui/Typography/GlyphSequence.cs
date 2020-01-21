using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class GlyphSequence : IEnumerable<Glyph> {

		private readonly List<Glyph> glyphs;

		public GlyphSequence(IEnumerable<Glyph> glyphs) {
			this.glyphs = new List<Glyph>(glyphs);
		}

		public int Count => glyphs.Count;

		public IEnumerator<Glyph> GetEnumerator() => glyphs.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => glyphs.GetEnumerator();

	}

}