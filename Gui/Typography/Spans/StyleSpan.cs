using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class StyleSpan : Span {

		public StyleSpan(int startIndex, int length) : base(startIndex, length) {
		}

		protected override string BaseToString() {
			throw new NotImplementedException();
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
			throw new NotImplementedException();
		}
	}

}