using Nova.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.Gui.Text {

	public static class TextSplitter {

		private struct Fragment {
			public string Literal { get; }
			public FontString Characters { get; }
			public int Width { get; }

			public Fragment(string literal, FontString characters, int width) {
				Literal = literal;
				Characters = characters;
				Width = width;
			}

			public override string ToString() {
				return $"Fragment['{Literal.Replace("\n", "\\n")}' {Width}]";
			}
		}

		public static readonly Regex RegexSplit = new Regex(@"\S+|\n|[ ]+");

		public static FontMultistring SplitText(Font font, string str, int maxWidth) {

			var fragments = new List<Fragment>();


			foreach (Match match in RegexSplit.Matches(str)) {

				var characters = new FontString(font.GetCharacters(match.Value));
				fragments.Add(new Fragment(match.Value, characters, characters.GetWidth(font)));

			}

			return SplitText(font, fragments, maxWidth);
		}

		private static FontMultistring SplitText(Font font, List<Fragment> fragments, int maxWidth) {

			FontMultistring output = new FontMultistring();

			int lineWidth = 0;
			FontString line = new FontString();

			System.IO.File.WriteAllText(@"C:\Users\Bryan\Desktop\Output.txt", fragments.ToPrettyString(true));

			foreach (var frag in fragments) {


				if (frag.Literal == "\n") {
					output.Add(line);
					line = new FontString();
					lineWidth = 0;

				} else if (frag.Width > maxWidth) {

					output.Add(line);
					output.AddRange(SplitFragment(font, frag.Characters, maxWidth, out line));
					lineWidth = line.GetWidth(font);

				} else {

					if (lineWidth + frag.Width <= maxWidth) {
						// Line has space for current fragment. Add line and continue to next fragment.
						line.Add(frag.Characters);
						lineWidth += frag.Width;

					} else {
						// Adding fragment will make current line too long. Split line.
						output.Add(line);

						if (!string.IsNullOrWhiteSpace(frag.Literal)) {
							// The fragment contains actual text. Write text immediately.
							line = new FontString(frag.Characters);
							lineWidth = frag.Width;
						} else {
							// The fragment was whitespace. Make newline but don't write whitespace.
							line = new FontString();
							lineWidth = 0;
						}

					}

				}

			}

			// Flush rest of current line to output
			output.Add(line);

			return output;
		}

		private static FontMultistring SplitFragment(Font font, FontString characters, int maxWidth, out FontString newCurrent) {

			FontMultistring output = new FontMultistring();
			FontString line = new FontString();

			for (int i = 0; i < characters.Count; i++) {

				int xAdvance = characters[i].XAdvance;
				if (i < characters.Count - 1) {
					xAdvance += font.GetKerning(characters[i].Character, characters[i + 1].Character);
				}

				if (line.GetWidth(font) + xAdvance <= maxWidth) {
					line.Add(characters[i]);
				} else {
					output.Add(line);
					line = new FontString() { characters[i] };
				}

			}

			newCurrent = line;

			return output;

		}



	}

}