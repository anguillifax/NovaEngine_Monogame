using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	public class TypographData {

		public string PlainText { get; set; }

		public Locale Locale { get; }

		internal List<IToken> Tokens { get; }
		internal List<Span> Spans { get; }

		public TypographData(string plaintext, Locale locale, IEnumerable<IToken> tokens = null, IEnumerable<Span> spans = null) {

			PlainText = plaintext ?? throw new ArgumentNullException("PlainText argument cannot be null");
			Locale = locale ?? throw new ArgumentNullException("Locale argument cannot be null");

			Tokens = tokens == null ? new List<IToken>() : new List<IToken>(tokens);
			Spans = spans == null ? new List<Span>() : new List<Span>(spans);

		}

		public void Add(Span span) => Spans.Add(span);
		public void Add(IToken token) => Tokens.Add(token);

	}

}