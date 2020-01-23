using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	public enum OverflowBehavior {
		Extend, Wrap
	}

	/// <summary>
	/// Controls typograph wrapping and display transformations.
	/// </summary>
	public class TypographDisplayProperties {

		public OverflowBehavior OverflowBehavior { get; set; }
		public float MaxWidth { get; set; }
		public Matrix TransformMatrix { get; set; }

		public TypographDisplayProperties(OverflowBehavior overflowBehavior, float maxWidth = float.MaxValue) {
			OverflowBehavior = overflowBehavior;
			MaxWidth = maxWidth;
			TransformMatrix = Matrix.Identity;
		}

	}

}