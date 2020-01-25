using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.Gui.Typography {

	public static class LoadFromMarkup {

		public class MarkupSyntaxError : Exception {
			public MarkupSyntaxError() {
			}

			public MarkupSyntaxError(string message) : base(message) {
			}

			public MarkupSyntaxError(string message, Exception innerException) : base(message, innerException) {
			}
		}

		/// <summary>
		/// Matches parameterized and unparameterized opening spans.
		/// </summary>
		private static readonly Regex MatchSpanOpen = new Regex(@"<(?<type>\w+)(?:\s+'(?<parameter>.*?)')?>");

		/// <summary>
		/// Matches closing spans.
		/// </summary>
		private static readonly Regex MatchSpanClose = new Regex(@"<\/(?<type>\w+)>");

		/// <summary>
		/// Matches parameterized and unparameterized tokens.
		/// </summary>
		private static readonly Regex MatchToken = new Regex(@"\{(?<type>\w+) ?(?:'(?<parameter>.*?)')?\}");

		/// <summary>
		/// Matches any tag-like object.
		/// </summary>
		private static readonly Regex MatchAny = new Regex(@"<.*?>|\{.*?\}");

		private static readonly Regex MatchVector2 = new Regex(@"(?<x>\d+),\s*(?<y>\d+)");


		private static Span GetSpan(string name, int startIndex, int length, string parameter) {
			switch (name) {
				case "color":
					return new ColorSpan(startIndex, length, ColorUtil.FromHex(parameter));
				case "jitter":
					return new JitterSpan(startIndex, length, float.Parse(parameter));
				case "nobreak":
					return new NonBreakingSequenceSpan(startIndex, length);
				case "rainbow":
					return string.IsNullOrEmpty(parameter) ? new RainbowColorSpan(startIndex, length) : new RainbowColorSpan(startIndex, length, cycleTime: float.Parse(parameter));
				case "style":
					return new StyleSpan(startIndex, length, parameter);
				case "offset":
					Match m = MatchVector2.Match(parameter);
					return new OffsetSpan(startIndex, length, new Vector2(float.Parse(m.Groups["x"].Value), float.Parse(m.Groups["y"].Value)));

				default:
					return null;
			}
		}

		private static Token GetToken(string name, int index, string parameter) {
			switch (name) {
				case "insertion":
					return new InsertionToken(index, parameter);
				case "symbol":
					return new ExternalSymbolToken(index, parameter);

				default:
					return null;
			}
		}

		/// <summary>
		/// Parses raw markup text into a fully formed TypographData.
		/// </summary>
		/// <exception cref="MarkupSyntaxError" />
		public static TypographData Load(string markupText, Localization localization = null, bool showLog = false) {

			string plainText = MatchAny.Replace(markupText, "");

			if (showLog) {
				Console.WriteLine("\nParsing from Markup\n");
				MFormat.PrintIndented(1, $"PlainText: {plainText}");
				Console.WriteLine();
			}

			var items = new List<Tuple<int, IElement>>();

			var openingSpans = MatchSpanOpen.Matches(markupText);

			var closingSpans = new Dictionary<string, List<Match>>();
			foreach (Match m in MatchSpanClose.Matches(markupText)) {
				string type = m.Groups["type"].Value;
				if (closingSpans.TryGetValue(type, out var matches)) {
					matches.Add(m);
				} else {
					closingSpans[type] = new List<Match>() { m };
				}
			}
			//MPrint.PrintIndented(1, closingSpans.ToPrettyString(x => x, x => x.ToPrettyString())); 

			var spanLengths = new Dictionary<Match, int>(openingSpans.Count);

			for (int i = 0; i < openingSpans.Count; ++i) {

				Match curOpen = openingSpans[i];
				string type = curOpen.Groups["type"].Value;

				Match curClose;
				try {
					curClose = closingSpans[type][0];
				} catch (Exception) {
					throw new MarkupSyntaxError($"Failed to find close tag for {curOpen}.");
				}
				if (curClose.Index < curOpen.Index + curOpen.Value.Length) throw new MarkupSyntaxError($"Close tag for {curOpen} found before start tag");

				closingSpans[type].RemoveAt(0);

				spanLengths[curOpen] = MatchAny.Replace(markupText.Substring(curOpen.Index, curClose.Index - curOpen.Index), "").Length;

			}

			foreach (var item in closingSpans) {
				if (item.Value.Count > 0) {
					MFormat.PrintIndented(1, $"[Warning] {item.Value.Count} extra unmatched closing tag(s) of type </{item.Key}>");
				}
			}

			if (showLog) Console.WriteLine("  Span Matches");

			foreach (Match m in openingSpans) {
				Span span = GetSpan(
					m.Groups["type"].Value, 
					MatchAny.Replace(markupText.Substring(0, m.Index), "").Length,
					spanLengths[m],
					m.Groups["parameter"].Value);
				if (span != null) {
					if (showLog) MFormat.PrintIndented(2, $"[{m.Index}] {m.Value}");
					items.Add(new Tuple<int, IElement>(m.Index, span));
				}
			}

			if (showLog) Console.WriteLine("  Token Matches");

			foreach (Match m in MatchToken.Matches(markupText)) {
				Token token = GetToken(m.Groups["type"].Value, 
					MatchAny.Replace(markupText.Substring(0, m.Index), "").Length, 
					m.Groups["parameter"].Value);
				if (token != null) {
					if (showLog) MFormat.PrintIndented(2, $"[{m.Index}] {m.Groups["type"]}: {m.Value}");
					items.Add(new Tuple<int, IElement>(m.Index, token));
				}
			}
			if (showLog) Console.WriteLine();

			items.Sort();

			int digits = items.Last().Item1.ToString().Length;
			if (showLog) MFormat.PrintIndented(1, "Items", items, x => $"({x.Item1.ToString().PadLeft(digits)}, {x.Item2})");

			if (showLog) Console.WriteLine("Finished Parsing Markup\n");

			return new TypographData(plainText, localization, items.Select(x => x.Item2));
		}

	}

}