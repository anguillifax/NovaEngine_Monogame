using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public class JitterSpan : UpdateSpan {

		private const float Delay = 0.05f;

		public float Strength { get; set; }
		private readonly List<Vector2> offsets;
		private float coolDown;
		private readonly Random random;

		private int curIndex;

		public JitterSpan(float strength) : this(0, 0, strength) { }

		public JitterSpan(int startIndex, int length, float strength) :
			base(startIndex, length) {
			Strength = strength;
			coolDown = 0;
			random = new Random();
			offsets = new List<Vector2>();
		}

		public override Span CloneSpan() => new JitterSpan(StartIndex, Length, Strength);

		internal override void Initialize(Typograph typograph, int glyphIndex, Glyph glyph) {
			offsets.Add(Vector2.Zero);
		}

		internal override void Update() {
			coolDown += Time.DeltaTime;
			if (coolDown > Delay) {
				coolDown -= Delay;

				for (int i = 0; i < offsets.Count; ++i) {
					offsets[i] = GenerateOffset();
				}
			}
			curIndex = 0;
		}

		internal override void Apply(Typograph typograph, int glyphIndex, Glyph glyph) {
			glyph.Offset += offsets[curIndex++];
		}

		private Vector2 GenerateOffset() {
			return new Vector2(Get(), Get());
		}

		private float Get() {
			return Strength * (random.Next(0, 2) == 0 ? -1f : 1f) * Calc.Remap((float)random.NextDouble(), 0, 1, -.3f, 1f);
		}

		protected override string BaseToString() => $"Jitter ({Strength:f2})";

	}

}