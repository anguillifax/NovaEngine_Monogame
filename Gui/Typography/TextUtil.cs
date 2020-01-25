using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	internal static class TextUtil {

		/// <summary>
		/// Replaces all new line sequences with single \n line feed.
		/// </summary>
		public static string NormalizeLineEnding(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n');

		/// <summary>
		/// Get the character in U+0000 unicode format.
		/// </summary>
		public static string GetUnicodePoint(char c) {
			return $"U+{(int)c:X4}";
		}

		/// <summary>
		/// Replaces \n new line characters with a literal "\n".
		/// </summary>
		public static string GetRepresentation(string s) {
			return s.Replace("\n", @"\n");
		}

	}

}