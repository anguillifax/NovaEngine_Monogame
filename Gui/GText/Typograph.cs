using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	public enum OverflowBehavior {
		Extend, Wrap
	}

	public class Typograph {

		private Glyph[] Glyphs { get; }

		public Font Font { get; }
		public string PlainText { get; }

		public Vector2 TopLeft { get; set; }

		public Typograph(Font font, string text, Vector2 topLeft) {

			Font = font ?? throw new ArgumentNullException("Font cannot be null");
			PlainText = text ?? throw new ArgumentNullException("String cannot be null");

			TopLeft = topLeft;

			Glyphs = new Glyph[text.Length];

			Resize();

		}

		public void Resize(OverflowBehavior overflowBehavior = OverflowBehavior.Extend, float maxWidth = float.MaxValue) {

			switch (overflowBehavior) {
				case OverflowBehavior.Extend:

					Vector2 pos = TopLeft;

					for (int i = 0; i < PlainText.Length; ++i) {

						Glyph g = Font.GetGlyph(PlainText[i]);

						g.Color = Color.MonoGameOrange;

						g.Position = pos;
						pos.X += g.Data.XAdvance;

						if (i > 0) {
							g.Position += new Vector2(Font.GetKerning(PlainText[i - 1], PlainText[i]), 0);
						}

						Glyphs[i] = g;
					}

					break;
				case OverflowBehavior.Wrap:

					// splitter

					break;
			}

		}

		public void Update() {

		}

		public void Render() {
			MDraw.Begin();
			foreach (var g in Glyphs) {
				g.Render();
			}
			MDraw.End();
		}

		public override string ToString() {
			return $"Typograph ({PlainText})";
		}

	}

}