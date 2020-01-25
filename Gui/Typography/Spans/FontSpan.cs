namespace Nova.Gui.Typography.Spans {

	public class FontSpan : Span {

		public Font Font { get; set; }

		public FontSpan(Font font) : this(0, 0, font) { }

		public FontSpan(int startIndex, int length, Font font) :
			base(startIndex, length) {
			Font = font;
		}

		public override Span CloneSpan() => new FontSpan(StartIndex, Length, Font);

		protected override string BaseToString() => $"Font '{Font}'";

	}

}