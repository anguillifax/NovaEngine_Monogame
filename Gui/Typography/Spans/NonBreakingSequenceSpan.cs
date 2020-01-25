namespace Nova.Gui.Typography {

	public class NonBreakingSequenceSpan : Span {

		public NonBreakingSequenceSpan() :
			this(0, 0) {
		}

		public NonBreakingSequenceSpan(int startIndex, int length) :
			base(startIndex, length) {
		}

		public override Span CloneSpan() => new NonBreakingSequenceSpan(StartIndex, Length);

		protected override string BaseToString() => "NonBreakingSequence";

	}

}