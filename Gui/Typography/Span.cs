﻿using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public abstract class Span : ICloneable {

		private int startIndex_;
		public int StartIndex {
			get => startIndex_;
			set {
				if (value < 0) throw new ArgumentOutOfRangeException("Index must be at least 0.");
				startIndex_ = value;
			}
		}

		private int length_;
		public int Length {
			get => length_;
			set {
				if (value < 0) throw new ArgumentOutOfRangeException("Length must be at least 0.");
				length_ = value;
			}
		}

		public int StopIndex => StartIndex + Length;

		protected Span(int startIndex, int length) {
			StartIndex = startIndex;
			Length = length;
		}

		public abstract Span CloneSpan();
		public object Clone() => CloneSpan();

		internal abstract void Initialize(Typograph typograph, GlyphSequence glyphs);

		public override string ToString() {
			return $"Span ({BaseToString()} [{StartIndex}, {Length}])";
		}

		protected abstract string BaseToString();

	}

	public abstract class UpdateSpan : Span {

		protected UpdateSpan(int startIndex, int length) : base(startIndex, length) {
		}

		internal abstract void Update(Typograph typograph, GlyphSequence glyphs);

	}

}