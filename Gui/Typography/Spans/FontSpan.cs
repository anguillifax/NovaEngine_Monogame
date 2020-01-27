namespace Nova.Gui.Typography {

	public class FontSpan : Span {

		public string FontKey { get; set; }

		public FontSpan(string font) : this(0, 0, font) { }

		public FontSpan(int startIndex, int length, string fontKey) :
			base(startIndex, length) {
			FontKey = fontKey;
		}

		public override Span CloneSpan() => new FontSpan(StartIndex, Length, FontKey);

		protected override string BaseToString() => $"Font '{FontKey}'";

	}

}