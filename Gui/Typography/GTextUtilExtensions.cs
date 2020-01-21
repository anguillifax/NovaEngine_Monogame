using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	internal static class GTextUtil {

		/// <summary>
		/// Replaces all new line sequences with single \n line feed.
		/// </summary>
		public static string NormalizeLineEnding(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n');

	}

	internal static class GTextUtilExtensions {

		public static string GetUnicodePoint(this char c) {
			return $"U+{(int)c:X4}";
		}

		public static string GetRepresentation(this string s) {
			return s.Replace("\n", @"\n");
		}

	}

}