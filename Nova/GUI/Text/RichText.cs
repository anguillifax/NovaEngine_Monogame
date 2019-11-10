using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Text {

	public class RichText : IDrawableText {

		public Font Font { get; }
		protected FontDraw FontDraw { get; }

		protected int maxWidth;

		public RichText(Font font, int maxPixelWidth, string text) {

			Font = font;

			maxWidth = maxPixelWidth;

			var lines = new List<List<FontCharacter>>();

			int curLineWidth = 0;
			int curWordWidth = 0;
			var curLine = new List<FontCharacter>();
			var curWord = new List<FontCharacter>();

			for (int i = 0; i < text.Length; i++) {

				if (IsSeparator(text[i])) {

					curLine.AddRange(curWord);
					curLineWidth += curWordWidth;

					curWord = new List<FontCharacter>();
					curWordWidth = 0;

					if (text[i] == '\n') {

						lines.Add(curLine);

						curLine = new List<FontCharacter>();
						curLineWidth = 0;

					}

				}

				if (font.TryGet(text[i], out FontCharacter fontChar)) {

					int advance = fontChar.XAdvance;
					if (i < text.Length - 1) {
						advance += font.GetKerning(text[i], text[i + 1]);
					}
					curWordWidth += advance;
					curWord.Add(fontChar);

					if (curLineWidth + curWordWidth > maxPixelWidth) {
						Console.WriteLine();
						Console.WriteLine($"Too long! Line would be {curLineWidth + curWordWidth} by adding {curWordWidth}");
						Console.WriteLine($"Added {curLine.ToPrettyString((x) => x.Character.ToString())} to final lines.");
						Console.WriteLine();

						lines.Add(curLine);

						curLine = new List<FontCharacter>();
						curLineWidth = 0;

					}

				}

				if (curLineWidth == 0) {

					while (curWord.Count > 1 && curWord[0].Character == ' ') {
						curWordWidth -= curWord[0].XAdvance + Font.GetKerning(curWord[0].Character, curWord[1].Character);
						Console.WriteLine("removing " + curWord[0]);
						curWord.RemoveAt(0);
					}

				}

			}

			Console.WriteLine("Adding end");
			Console.WriteLine(curLine.ToPrettyString((x) => x.Character.ToString()));
			lines.Add(curLine);

			Console.WriteLine();
			Console.WriteLine("Final output");
			Console.WriteLine(lines.ToPrettyString((x) => x.ToPrettyString((y) => y.Character.ToString())));

			FontDraw = new FontDraw(font, lines);

		}

		private bool IsSeparator(char c) {
			return c == ' ' || c == ',' || c == '\n';
		}

		public void Draw(Vector2 position, Color color, int depth) {
			MDraw.DrawRectGlobal(new FloatRect(position.X, position.Y, maxWidth, Font.LineHeight * FontDraw.Lines.Count), Color.White.Multiply(0.2f));
			FontDraw.Draw(position, color, depth);
		}
	}

}