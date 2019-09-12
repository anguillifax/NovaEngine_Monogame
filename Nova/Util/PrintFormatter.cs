using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova {

	/// <summary>
	/// Pretty ToString for various types
	/// </summary>
	public static class PrintFormatter {

		public static string JoinComma<T>(T[] arr) {
			return "{" + string.Join(", ", arr) + "}";
		}

		public static string JoinComma<T>(IEnumerable<T> e) {
			return "{" + string.Join(", ", e) + "}";
		}

		public static string ToPrettyString<T>(this T[] arr) {
			if (arr == null) return "{}";
			return JoinComma(arr);
		}

		public static string ToPrettyString<T>(this List<T> list) {
			if (list == null) return "{}";
			return JoinComma(list);
		}

		public static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dict) {
			if (dict == null) return "{}";
			return JoinComma(dict.Select((k) => string.Format("[{0}, {1}]", k.Key, k.Value)));
		}

	}

}