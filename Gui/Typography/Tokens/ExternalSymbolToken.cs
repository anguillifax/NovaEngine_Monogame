using System;

namespace Nova.Gui.Typography {

	public class ExternalSymbolToken : Token {

		public string Key { get; }

		public ExternalSymbolToken(int index, string key) :
			base(index) {
			Key = key ?? throw new ArgumentNullException("External symbol key cannot be null");
		}

		public override void Consume(Typograph typograph) {
		}

		protected override string BaseToString() => $"ExternalSymbol ({Key})";
	}

}