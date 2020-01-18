using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	internal static class GTextUtil {

		public static string GetUnicodePoint(this char c) {
			return $"U+{(int)c:X4}";
		}

		public static string GetRepresentation(this string s) {
			return s.Replace("\n", @"\n");
		}

	}

}