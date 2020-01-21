namespace Nova.Gui.Typography {

	public abstract class Token {

		public int Index { get; set; }

		protected Token(int index) {
			Index = index;
		}

		public abstract void Consume(Typograph typograph);

		public override string ToString() {
			return $"Token: {BaseToString()} [{Index}]";
		}

		protected abstract string BaseToString();

	}

}