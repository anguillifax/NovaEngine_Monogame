namespace Nova.Gui.GText {

	public abstract class Span {

		public int StartIndex { get; set; }
		public int Length { get; set; }
		public int StopIndex => StartIndex + Length;

		public static bool Contains(Span lhs, Span rhs) {
			throw new System.NotImplementedException();
		}

		protected Span(int startIndex, int length) {
			StartIndex = startIndex;
			Length = length;
		}

		internal abstract void Apply();

		public override string ToString() {
			return $"{BaseToString()} ({StartIndex}, {Length})";
		}

		protected abstract string BaseToString();

	}

	public abstract class UpdateSpan : Span {

		protected UpdateSpan(int startIndex, int length) : base(startIndex, length) {
		}

		public abstract void Update();

	}

}