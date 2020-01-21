using Microsoft.Xna.Framework;
using Nova.Util;

namespace Nova.Gui.Typography {

	public class ColorSpan : Span {

		public Color Color { get; set; }

		public ColorSpan(int startIndex, int length, Color color) : 
			base (startIndex, length) {
			Color = color;
		}

		public ColorSpan(int startIndex, int length, string hex) :
			this(startIndex, length, ColorUtil.FromHex(hex)) {
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
			foreach (var g in glyphs) {
				g.Color = Color;
			}
		}

		protected override string BaseToString() {
			return $"Color ({Color})";
		}

	}

}