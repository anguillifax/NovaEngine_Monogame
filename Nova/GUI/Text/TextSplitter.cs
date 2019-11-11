using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nova.Gui.Text {

	public static class TextSplitter {

		public static readonly Regex RegexWordsOrWhitespace = new Regex(@"\S+|\s+");

		public static List<List<FontCharacter>> SplitText(Font font, string str) {

			var fragments = new List<Fragment>();


			foreach (Match match in RegexWordsOrWhitespace.Matches(str)) {

				var characters = new List<FontCharacter>();
				//add characters

				//create fragment

			}

			return null;
		}

		private static List<List<FontCharacter>> SplitText(List<Fragment> fragments) {

			return null;
		}

		private struct Fragment {
			public string literal;
			public List<FontCharacter> characters;
			public int width;
		}

	}

}