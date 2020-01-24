using Microsoft.Xna.Framework;
using Nova.Util;

namespace Nova.Gui.Typography {

	public class ColorSpan : Span {

		public Color Color { get; set; }

		public ColorSpan(Color color) : this(0, 0, color) { }

		public ColorSpan(int startIndex, int length, Color color) : 
			base (startIndex, length) {
			Color = color;
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
			foreach (var g in glyphs) {
				g.Color = Color;
			}
		}

		public override Span CloneSpan() => new ColorSpan(StartIndex, Length, Color);

		protected override string BaseToString() {
			return $"Color ({Color.ToHex()})";
		}

	}

}