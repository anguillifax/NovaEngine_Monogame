using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Util {

	public static class ColorUtil {

		/// <summary>
		/// Get a color from HSV.
		/// </summary>
		/// <param name="hue">A float between [0, 360]</param>
		/// <param name="saturation">A float between [0,1]</param>
		/// <param name="value">A float between [0, 1]</param>
		public static Color FromHSV(float hue, float saturation, float value) {
			if (saturation == 0) {
				return new Color(value, value, value);
			}
			if (hue == 360) hue = 0;
			hue /= 60;
			int i = (int)hue;
			float f = hue - i;
			float p = value * (1 - saturation);
			float q = value * (1 - saturation * f);
			float t = value * (1 - saturation * (1 - f));

			switch (i) {
				case 0:
					return new Color(value, t, p);
				case 1:
					return new Color(q, value, p);
				case 2:
					return new Color(p, value, t);
				case 3:
					return new Color(p, q, value);
				case 4:
					return new Color(t, p, value);
				default:
					return new Color(value, p, q);
			}

		}

		/// <summary>
		/// Convert a color in the form #FFFFFF, #FFF, FFFFFF, or FFF.
		/// </summary>
		public static Color FromHex(string hex) {
			if (hex[0] == '#') hex = hex.Substring(1);
			if (hex.Length == 3) hex = new string(new char[] { hex[0], hex[0], hex[1], hex[1], hex[2], hex[2] });
			var c = new Color(
				int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
				int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
			return c;
		}

	}

}