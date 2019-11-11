using Microsoft.Xna.Framework;
using Nova.Util;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Text {

	public class FontDraw : IDrawableText {

		public Font Font { get; }
		public FontMultistring Lines { get; }

		public FontDraw(Font font, FontMultistring lines) {
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

}