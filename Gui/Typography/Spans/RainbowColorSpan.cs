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

		public RainbowColorSpan(float cycleTime = 3f) : this(0, 0, cycleTime) { }

		public RainbowColorSpan(int startIndex, int length, float cycleTime = 3f) :
			base(startIndex, length) {
			curHue = 0;
			CycleTime = cycleTime;
		}

		public override Span CloneSpan() => new RainbowColorSpan(StartIndex, Length, CycleTime);

		internal override void Apply(Typograph typograph, int glyphIndex, Glyph glyph) {
			glyph.Color = Util.ColorUtil.FromHSV(Calc.Loop(curHue + ChangePerCharacter * glyphIndex, 0, 360), 0.8f, 1f);
			curHue += 360f / (CycleTime * Engine.FrameRate);
		}

		internal override void Update() {
			curHue += 360f / (CycleTime * Engine.FrameRate);
		}

		protected override string BaseToString() => "RainbowColor";

	}

}