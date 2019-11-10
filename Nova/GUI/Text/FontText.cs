using Microsoft.Xna.Framework;
using Nova.Util;
using System.Collections.Generic;

namespace Nova.Gui.Text {

	public class FontDraw : IDrawableText {

		public Font Font { get; }
		public List<List<FontCharacter>> Lines { get; }

		public FontDraw(Font font, List<List<FontCharacter>> lines) {
			Font = font;
			Lines = lines;
		}

		public void Draw(Vector2 position, Color color, int depth) {

			Vector2 pos = position.Clone();

			foreach (var line in Lines) {
				for (int i = 0; i < line.Count; i++) {

					line[i].Draw(pos, color, depth);

					if (i < line.Count - 1) {
						pos.X += line[i].XAdvance + Font.GetKerning(line[i].Character, line[i + 1].Character);
					}
				}

				pos.X = position.X;
				pos.Y += Font.LineHeight;
			}

		}

	}

	/// <summary>
	/// Represents lines of text on screen.
	/// </summary>
	public class FontText : IDrawableText {

		public Font Font { get; }
		protected FontDraw FontDraw { get; }

		/// <summary>
		/// Create a new font string from a line of text.
		/// <para>Automatically splits newlines.</para>
		/// </summary>
		public FontText(Font font, string str) :
			this(font, false, str.Split('\n')) {
		}

		/// <summary>
		/// Create a new font string from lines of text.
		/// <para>If doSplit is true, constructor will split any newlines in text given. If doSplit is false, newlines will be ignored.</para>
		/// </summary>
		public FontText(Font font, bool doSplit, params string[] text) {

			Font = font;

			if (doSplit) {

				var split = new List<string>();
				foreach (var item in text) {
					split.AddRange(item.Split('\n'));
				}

				FontDraw = new FontDraw(Font, CreateLines(split));

			} else {

				FontDraw = new FontDraw(Font, CreateLines(text));
				
			}

		}

		private List<List<FontCharacter>> CreateLines(IEnumerable<string> text) {
			var lines = new List<List<FontCharacter>>();
			foreach (var line in text) {
				lines.Add(Font.GetCharacters(line));
			}
			return lines;
		}

		public void Draw(Vector2 position, Color color, int depth) {
			FontDraw.Draw(position, color, depth);
		}

	}

}