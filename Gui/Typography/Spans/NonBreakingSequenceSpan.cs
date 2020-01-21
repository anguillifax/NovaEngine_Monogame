using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class NonBreakingSequenceSpan : Span {

		public NonBreakingSequenceSpan(int startIndex, int length) :
			base(startIndex, length) {
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		protected override string BaseToString() => "NonBreakingSequence";

	}

}