using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Util {

	/// <summary>
	/// Pretty ToString for various types
	/// </summary>
	public static class StringF {

		/// <summary>
		/// Get a string representation of vector with <code>precision</code> digits after the point.
		/// </summary>
		public static string ToStringFixed(this Vector2 v, int precision = 2) {
			return "{" + v.X.ToString("f" + precision) + ", " + v.Y.ToString("f" + precision) + "}";
		}

		public static string JoinComma<T>(T[] arr, bool newline) {
			if (newline) {
				return "{\n" + string.Join(",\n", arr) + "\n}";
			} else {
				return "{" + string.Join(", ", arr) + "}";
			}
		}

		public static string JoinComma<T>(IEnumerable<T> e, bool newline) {
			if (newline) {
				return "{\n" + string.Join(",\n", e) + "\n}";
			} else {
				return "{" + string.Join(", ", e) + "}";
			}
		}

		public static string ToPrettyString<T>(this T[] arr, bool newline = false) {
			if (arr == null) return "{}";
			return JoinComma(arr, newline);
		}

		public static string ToPrettyString<T>(this IEnumerable<T> e, bool newline = false) {
			if (e == null) return "{}";
			return JoinComma(e, newline);
		}

		public static string ToPrettyString<T>(this T[] arr, Func<T, string> formatter, bool newline = false) {
			if (arr == null) return "{}";
			if (formatter == null) formatter = (x) => x.ToString();
			return JoinComma(arr.Select(formatter), newline);
		}

		public static string ToPrettyString<T>(this List<T> list, bool newline = false) {
			if (list == null) return "{}";
			return JoinComma(list, newline);
		}

		public static string ToPrettyString<T>(this List<T> list, Func<T, string> formatter, bool newline = false) {
			if (list == null) return "{}";
			if (formatter == null) formatter = (x) => x.ToString();
			return JoinComma(list.Select(formatter), newline);
		}

		public static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dict, bool newline = false) {
			if (dict == null) return "{}";
			return JoinComma(dict.Select((k) => string.Format("[{0}, {1}]", k.Key, k.Value)), newline);
		}

		/// <summary>
		/// Overrides the call to ToString() on the key and value. If formatter is null, then default ToString() is used.
		/// </summary>
		public static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dict, Func<TK, string> keyFormatter, Func<TV, string> valueFormatter, bool newline = false) {
			if (dict == null) return "{}";
			if (keyFormatter == null) keyFormatter = (x) => x.ToString();
			if (valueFormatter == null) valueFormatter = (x) => x.ToString();
			return JoinComma(dict.Select((k) => string.Format("[{0}, {1}]", keyFormatter(k.Key), valueFormatter(k.Value))), newline);
		}

		public static string ToIndentString<T>(this IEnumerable<T> o, string header) {
			return $"{header}\n" + string.Join("\n", o.Select(x => $"  {x}"));
		}

		private const int IndentAmount = 2;
		public static string ToIndented<T>(int level, string header, IEnumerable<T> objects, Func<T, string> conversion = null) {
			StringBuilder sb = new StringBuilder();
			sb.Append(' ', IndentAmount * level);
			sb.AppendLine(header);
			foreach (var item in objects) {
				sb.Append(' ', IndentAmount * (level + 1));
				sb.AppendLine(conversion != null ? conversion(item) : conversion.ToString());
			}
			return sb.ToString();
		}

		public static void PrintIndented<T>(int level, string header, IEnumerable<T> objects, Func<T, string> conversion = null) {
			Console.WriteLine(ToIndented(level, header, objects, conversion));
		}

		public static string ToIndented(int level, string text) {
			return new string(' ', IndentAmount * level) + text;
		}

		public static void PrintIndented(int level, string text) {
			Console.WriteLine(ToIndented(level, text));
		}

		public static string ToHex(this Color color) {
			return $"#{color.R:X}{color.G:X}{color.B:X}{(color.A != 255 ? color.A.ToString("X") : string.Empty)}";
		}

	}

}