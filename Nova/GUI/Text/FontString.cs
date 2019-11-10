using Microsoft.Xna.Framework;
using Nova.Util;
using System.Collections.Generic;

namespace Nova.Gui.Text {

	/// <summary>
	/// Represents lines of text on screen.
	/// </summary>
	public class FontString {

		public List<List<FontCharacter>> Lines { get; }

		public Font Font { get; }

		/// <summary>
		/// Create a new font string from a line of text.
		/// <para>Automatically splits newlines.</para>
		/// </summary>
		public FontString(Font font, string str) :
			this(font, false, str.Split('\n')) {
		}

		/// <summary>
		/// Create a new font string from lines of text.
		/// <para>Automatically splits newlines.</para>
		/// </summary>
		public FontString(Font font, params string[] text) :
			this(font, true, text) {
		}

		/// <summary>
		/// Create a new font string from lines of text.
		/// <para>If doSplit is true, constructor will split any newlines in text given. If doSplit is false, newlines will be ignored.</para>
		/// </summary>
		public FontString(Font font, bool doSplit, params string[] text) {

			Font = font;
			Lines = new List<List<FontCharacter>>();

			if (doSplit) {

				var all = new List<string>();
				foreach (var item in text) {
					all.AddRange(item.Split('\n'));
				}
				CreateLines(all);

			} else {

				CreateLines(text);

			}

		}

		private void CreateLines(IEnumerable<string> lines) {
			foreach (var line in lines) {
				Lines.Add(Font.GetCharacters(line));
			}
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

}