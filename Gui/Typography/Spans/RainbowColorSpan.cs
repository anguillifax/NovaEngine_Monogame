using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class RainbowColorSpan : UpdateSpan {

		private float curHue;
		private const float ChangePerCharacter = -5f;

		/// <summary>
		/// How long it takes to make one full color revolution.
		/// </summary>
		public float CycleTime { get; set; }

		public RainbowColorSpan(int startIndex, int length, float startHue = 0, float cycleTime = 3f) :
			base(startIndex, length) {
			curHue = startHue;
			CycleTime = cycleTime;
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
		}

		internal override void Update(Typograph typograph, GlyphSequence glyphs) {
			int indexOffset = 0;
			foreach (var g in glyphs) {
				g.Color = Util.ColorUtil.FromHSV(Calc.Loop(curHue + ChangePerCharacter * indexOffset++, 0, 360), 0.8f, 1f);
			}
			curHue += 360f / (CycleTime * Engine.FrameRate);
		}

		protected override string BaseToString() => "RainbowColor";

	}

}