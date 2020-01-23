using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class StyleSpan : Span {

		public string Key { get; }

		public StyleSpan(string key) : this(0, 0, key) { }

		public StyleSpan(int startIndex, int length, string key) : 
			base(startIndex, length) {
			Key = key;
		}

		public override Span CloneSpan() => new StyleSpan(StartIndex, Length, Key);

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		protected override string BaseToString() => $"Style '{Key}'";

	}

}