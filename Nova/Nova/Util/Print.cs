using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project {

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