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

				Console.WriteLine(text[i]);

				if (text[i] == ' ' || text[i] == '\t' || text[i] == '\n') {

					if (curLine.Count + curWord.Count == 0) {

						if (text[i] == '\n') {
							Console.WriteLine("Added empty line");
							lines.Add(new List<FontCharacter>());
						}

						Console.WriteLine("Skipping whitespace as first character");
						continue;
					}

					Console.WriteLine("Split and write: " + curWord.ToPrettyString(x => x.Character.ToString()));

					curLine.AddRange(curWord);
					curLineWidth += curWordWidth;

					curWord = new List<FontCharacter>();
					curWordWidth = 0;

					if (text[i] == '\n') {
						Console.WriteLine("Newline character");
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

						// Trim front whitespace.

						Console.WriteLine();
						Console.WriteLine($"Too long! Line would be {curLineWidth + curWordWidth} by adding {curWordWidth}");
						Console.WriteLine($"Added {curLine.ToPrettyString(x => x.Character.ToString())} to final lines.");
						Console.WriteLine();

						lines.Add(curLine);

						curLine = new List<FontCharacter>();
						curLineWidth = 0;

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

		public void Draw(Vector2 position, Color color, int depth) {
			MDraw.DrawRectGlobal(new FloatRect(position.X, position.Y, maxWidth, Font.LineHeight * FontDraw.Lines.Count), Color.White.Multiply(0.2f));
			FontDraw.Draw(position, color, depth);
		}
	}

}