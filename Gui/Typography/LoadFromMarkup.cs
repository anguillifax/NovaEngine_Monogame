using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.Gui.Typography {

	public static class LoadFromMarkup {

		/// <summary>
		/// Matches parameterized and unparameterized spans.
		/// </summary>
		private static readonly Regex MatchSpan = new Regex(@"<(?<otype>\w+)(?:\s+'(?<parameter>.*?)')?>|<(?<isclose>\/)(?<ctype>\w+)>");

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
				case "font":
					return new FontSpan(startIndex, length, parameter);
				case "fontsize":
					return new FontSizeSpan(startIndex, length, float.Parse(parameter));

				default:
					Console.WriteLine($"[Warning] Span of type {name} was not recognized.");
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
					Console.WriteLine($"[Warning] Token of type {name} was not recognized.");
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
				MFormat.PrintIndented(1, $"PlainText: {TextUtil.GetRepresentation(plainText)}");
				Console.WriteLine();
			}


			var elements = new List<Tuple<int, IElement>>();



			Dictionary<string, Stack<Match>> openSpans = new Dictionary<string, Stack<Match>>();

			if (showLog) Console.WriteLine("  Span Matches");

			foreach (Match m in MatchSpan.Matches(markupText)) {
				if (!m.Groups["isclose"].Success) {

					string type = m.Groups["otype"].Value;

					if (openSpans.TryGetValue(type, out Stack<Match> stack)) {
						openSpans[type].Push(m);
					} else {
						openSpans.Add(type, new Stack<Match>());
						openSpans[type].Push(m);
					}

				} else {

					string type = m.Groups["ctype"].Value;

					if (openSpans.TryGetValue(type, out Stack<Match> stack)) {

						if (stack.Count > 0) {

							Match open = stack.Pop();

							Span span = GetSpan(type,
								MatchAny.Replace(markupText.Substring(0, open.Index), "").Length,
								MatchAny.Replace(markupText.Substring(open.Index + open.Length, m.Index - open.Index - open.Length), "").Length,
								open.Groups["parameter"].Value);

							if (span != null) {
								if (showLog) MFormat.PrintIndented(2, $"[{open.Index}] {span}");
								elements.Add(new Tuple<int, IElement>(open.Index, span));
							}

							continue;

						}

					}

					Console.WriteLine($"  [Warning] Unmatched close spans '{m.Value}'. Ignoring span.");

				}

			}

			if (openSpans.Any(x => x.Value.Count > 0)) {

				Console.WriteLine("  [Warning] Unmatched open spans. Listing...");

				foreach (var item in openSpans) {
					foreach (var m in item.Value) {
						Console.WriteLine($"    [{m.Index}] {item.Key}: {m.Value}");
					}
				}

			}



			if (showLog) Console.WriteLine("  Token Matches");

			foreach (Match m in MatchToken.Matches(markupText)) {
				Token token = GetToken(m.Groups["type"].Value,
					MatchAny.Replace(markupText.Substring(0, m.Index), "").Length,
					m.Groups["parameter"].Value);
				if (token != null) {
					if (showLog) MFormat.PrintIndented(2, $"[{m.Index}] {token}");
					elements.Add(new Tuple<int, IElement>(m.Index, token));
				}
			}
			if (showLog) Console.WriteLine();

			elements.Sort();

			int digits = elements.Last().Item1.ToString().Length;
			if (showLog) MFormat.PrintIndented(1, "Items", elements, x => $"({x.Item1.ToString().PadLeft(digits)}, {x.Item2})");

			if (showLog) Console.WriteLine("\nFinished Parsing Markup\n");

			return new TypographData(plainText, localization, elements.Select(x => x.Item2));
		}

	}

}