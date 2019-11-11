using Microsoft.Xna.Framework;
using Nova.Util;
using System.Collections.Generic;

namespace Nova.Gui.Text {

	/// <summary>
	/// Represents lines of text on screen.
	/// </summary>
	public class BasicText : IDrawableText {

		public Font Font { get; }
		protected FontDraw FontDraw { get; }

		/// <summary>
		/// Create a new font string from a line of text.
		/// <para>Automatically splits newlines.</para>
		/// </summary>
		public BasicText(Font font, string str) :
			this(font, false, str.Split('\n')) {
		}

		/// <summary>
		/// Create a new font string from lines of text.
		/// <para>If doSplit is true, constructor will split any newlines in text given. If doSplit is false, newlines will be ignored.</para>
		/// </summary>
		public BasicText(Font font, bool doSplit, params string[] text) {

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

		private FontMultistring CreateLines(IEnumerable<string> text) {
			var lines = new FontMultistring();
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