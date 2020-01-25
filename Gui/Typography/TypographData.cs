using Nova.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	/// <summary>
	/// Contains configuration data to create a typograph.
	/// </summary>
	public class TypographData {

		/// <summary>
		/// Represents a typograph with no text, no localization, and no tokens or spans.
		/// </summary>
		public static readonly TypographData Empty = new TypographData("", null);

		public string PlainText { get; }

		public bool AttachedToLibrary => Localization != null;
		public Localization Localization { get; }

		public ElementCollection Elements { get; }

		public TypographData(string plaintext, Localization localization, IEnumerable<IElement> elements = null) {

			PlainText = plaintext ?? throw new ArgumentNullException("PlainText argument cannot be null");
			Localization = localization;

			Elements = elements == null ? new ElementCollection() : new ElementCollection(elements);

		}

		public TypographData(string plaintext, Localization localization, params IElement[] elements) :
			this(plaintext, localization, elements.AsEnumerable()) {
		}

		public override string ToString() {
			return $"TypographData (Spans: {Elements.SpanCount}, Tokens: {Elements.TokenCount}, {PlainText})";
		}

		public string ToStringDebug(int indent = 0) => MFormat.ToIndented(indent, "Typograph Data", Elements);


	}

}