namespace Nova.Gui.GText {

	public class NonBreakingSequence : Span {

		public NonBreakingSequence(int startIndex, int length) :
			base(startIndex, length) {
		}

		internal override void Apply() {
		}

		protected override string BaseToString() {
			return "NonBreakingSequence";
		}

	}

}