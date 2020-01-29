namespace Nova.Gui.Typography {

	public class FontSizeSpan : Span {

		public float Size { get; set; }

		public FontSizeSpan(float fontSize) : this(0, 0, fontSize) { }

		public FontSizeSpan(int startIndex, int length, float fontSize) :
			base(startIndex, length) {
			Size = fontSize;
		}

		public override Span CloneSpan() => new FontSizeSpan(StartIndex, Length, Size);

		protected override string BaseToString() => $"FontSize {Size:f1}";

	}

}