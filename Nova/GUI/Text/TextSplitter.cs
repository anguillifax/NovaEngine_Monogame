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

		public static readonly Regex RegexSplit = new Regex(@"\S+|\n|[ \t]+");

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

				//System.Console.WriteLine($"Width {lineWidth}:\t{frag}");

				if (frag.Literal == "\n") {
					System.Console.WriteLine("NEWLINE");
					output.Add(line);
					line = new FontString();
					lineWidth = 0;

				} else if (frag.Width > maxWidth) {

					//if (lineWidth != 0) {
					//	output.Add(line);
					//	line = new FontString();
					//}

					//for (int i = 0; i < frag.Characters.Count; i++) {

					//	for (int j = 0; j < frag.Characters.Count - i; j++) {

					//		var sub = frag.Characters.SubString(i, j);
					//		if (sub.GetWidth(font) > maxWidth) {
					//			output.Add(frag.Characters.SubString(i, j - 1));
					//			i = j;
					//			break;
					//		}

					//	}
					//}

				} else {

					if (lineWidth == 0 && string.IsNullOrWhiteSpace(frag.Literal)) {

						System.Console.WriteLine("Skipping fragment: " + frag);
						// Don't add empty space to front of line.		

					} else {

						if (lineWidth + frag.Width <= maxWidth) {
							line.Add(frag.Characters);
							lineWidth += frag.Width;

						} else {
							output.Add(line);

							System.Console.WriteLine("split line when reading " + frag);
							System.Console.WriteLine("CONTENTS\n" + output);
							System.Console.WriteLine();

							line = new FontString(frag.Characters);
							lineWidth = frag.Width;

						}

					}

				}

			}

			// Flush current line to output
			output.Add(line);

			return output;
		}



	}

}