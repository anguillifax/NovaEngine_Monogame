using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nova.Gui.Typography {

	public static class LoadFromMarkup {

		/// <summary>
		/// Matches parameterized and unparameterized spans.
		/// </summary>
		private static readonly Regex MatchSpan = new Regex(@"(?<open><(?<name>\w+) ?(?:'(?<parameter>.*?)')?>)(?=(?<contents>.*?)(?<close><\/\2>))");

		/// <summary>
		/// Matches only spans with parameterized matching start and close tags.
		/// </summary>
		private static readonly Regex MatchSpanMirrored = new Regex(@"(?<open><(?<name>\w+) '(?<parameter>.*?)'>)(?=(?<contents>.*?)(?<close><\/\2 '\3'>))");

		/// <summary>
		/// Matches parameterized and unparameterized tokens.
		/// </summary>
		private static readonly Regex MatchToken = new Regex(@"\{(?<name>\w+) ?(?:'(?<parameter>.*?)')?\}");

		/// <summary>
		/// Matches any tag-like object.
		/// </summary>
		private static readonly Regex MatchAny = new Regex(@"<.*?>|\{.*?\}");

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

		public static TypographData Load(string text, Localization localization = null, bool showLog = false) {

			TypographData d = new TypographData(MatchAny.Replace(text, ""), localization);

			if (showLog) {
				Console.WriteLine("Loading from markup...");
				Console.WriteLine($"| {d.PlainText}");
				Console.WriteLine("| Span Matches");
			}

			foreach (Match m in MatchSpan.Matches(text)) {
				Span span = GetSpan(m.Groups["name"].Value, MatchAny.Replace(text.Substring(0, m.Index), "").Length, MatchAny.Replace(m.Groups["contents"].Value, "").Length, m.Groups["parameter"].Value);
				if (span != null) {
					if (showLog) Console.WriteLine($"|  [{m.Index}] {m.Groups["name"]}: {m.Groups["open"]} (...) {m.Groups["close"]}");
					d.Add(span);
				}
			}

			if (showLog) Console.WriteLine("| Token Matches");
			foreach (Match m in MatchToken.Matches(text)) {
				Token token = GetToken(m.Groups["name"].Value, MatchAny.Replace(text.Substring(0, m.Index), "").Length, m.Groups["parameter"].Value);
				if (token != null) {
					if (showLog) Console.WriteLine($"|  [{m.Index}] {m.Groups["name"]}: {m.Value}");
					d.Add(token);
				}
			}
			if (showLog) Console.WriteLine("Finished Load.\n");

			return d;
		}

	}

}