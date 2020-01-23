using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class NonBreakingSequenceSpan : Span {

		public NonBreakingSequenceSpan() : this(0, 0) { }

		public NonBreakingSequenceSpan(int startIndex, int length) :
			base(startIndex, length) {
		}

		public override Span CloneSpan() => new NonBreakingSequenceSpan(StartIndex, Length);

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		protected override string BaseToString() => "NonBreakingSequence";

	}

}