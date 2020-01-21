using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class TypographData {

		/// <summary>
		/// Represents a typograph with no text, no localization, and no tokens or spans.
		/// </summary>
		public static readonly TypographData Empty = new TypographData("", null);

		public string PlainText { get; }

		public Localization Localization { get; }

		internal TokenCollection Tokens { get; }
		internal SpanCollection Spans { get; }

		public TypographData(string plaintext, Localization localization, IEnumerable<Token> tokens = null, IEnumerable<Span> spans = null) {

			PlainText = plaintext ?? throw new ArgumentNullException("PlainText argument cannot be null");
			Localization = localization;

			Tokens = tokens == null ? new TokenCollection() : new TokenCollection(tokens);
			Spans = spans == null ? new SpanCollection() : new SpanCollection(spans);

		}

		public void Add(Span span) => Spans.Add(span);
		public void Add(Token token) => Tokens.Add(token);

		private const int MaxVisibleCharacters = 40;
		public override string ToString() {
			return $"TypographData (Spans: {Spans.Count}, Tokens: {Tokens.Count}, " +
				$"'{(PlainText.Length < MaxVisibleCharacters ? PlainText.Substring(0, PlainText.Length) : (PlainText.Substring(0, MaxVisibleCharacters - 3) + "..."))}')";
		}

		public string ToStringDebug() => $"\n=== TypographData Printout ===\n\n{PlainText}\n\n{Spans}\n\n{Tokens}\n\n=== End Printout ===\n";


	}

}