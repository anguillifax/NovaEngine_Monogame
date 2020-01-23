using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class InsertionToken : Token {

		public string Key { get; }

		public InsertionToken(int index, string key) :
			base(index) {
			Key = key;
		}

		public override Token CloneToken() => new InsertionToken(Index, Key);

		public override void Consume(Typograph typograph) {
		}

		protected override string BaseToString() => $"Insertion '{Key}'";

	}

}