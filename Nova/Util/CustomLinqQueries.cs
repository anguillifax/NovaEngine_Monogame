using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Linq {

	public static class CustomLinqQueries {

		/// <summary>
		/// Returns the index of the smallest value.
		/// </summary>
		public static int IndexOfMin(this IEnumerable<float> e) {

			float smallest = float.MaxValue;
			int index = -1;

			for (int i = 0; i < e.Count(); i++) {
				if (e.ElementAt(i) < smallest) {
					smallest = e.ElementAt(i);
					index = i;
				}
			}

			return index;
		}

	}

}