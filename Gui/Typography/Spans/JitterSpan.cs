using Microsoft.Xna.Framework;
using System;

namespace Nova.Gui.Typography {

	public class JitterSpan : UpdateSpan {

		private const float Delay = 0.05f;

		public float Strength { get; set; }
		private Vector2[] offsets;
		private float coolDown;
		private readonly Random random;

		public JitterSpan(int startIndex, int length, float strength) :
			base(startIndex, length) {
			Strength = strength;
			coolDown = 0;
			random = new Random();
		}

		internal override void Initialize(Typograph typograph, GlyphSequence glyphs) {
			offsets = new Vector2[glyphs.Count];
		}

		internal override void Update(Typograph typograph, GlyphSequence glyphs) {

			coolDown += Time.DeltaTime;
			if (coolDown > Delay) {
				coolDown -= Delay;
				for (int i = 0; i < offsets.Length; ++i) {
					offsets[i] = GenerateOffset();
				}
			}

			int j = 0;
			foreach (var g in glyphs) {
				g.Offset += offsets[j++];
			}
		}

		private Vector2 GenerateOffset() {
			return new Vector2(Get(), Get());
		}

		private float Get() => Strength * Calc.Remap((float)random.NextDouble(), 0, 1, 0.3f, 1f);

		protected override string BaseToString() => $"Jitter ({Strength:f2})";

	}

}