using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Project {

	/// <summary>
	/// Pretty ToString for various types
	/// </summary>
	public static class PrintFormatter {

		public static string ArrayToString<T>(T[] arr) {
			return "{" + string.Join(", ", arr) + "}";
		}

		public static string ListToString<T>(List<T> list) {
			if (list == null) return "{}";
			return ArrayToString(list.ToArray());
		}

	}

}