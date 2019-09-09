using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public static class CustomExtensions {

		public static Vector2 Copy(this Vector2 vec) {
			return new Vector2(vec.X, vec.Y);
		}

		public static Vector3 Copy(this Vector3 vec) {
			return new Vector3(vec.X, vec.Y, vec.Z);
		}

		/// <summary>
		/// If dictionary contains key, existing value is updated to new value. If key is not in dictionary, it is added.
		/// </summary>
		public static void AddUpdate<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV value) {
			if (dict.ContainsKey(key)) {
				dict[key] = value;
			} else {
				dict.Add(key, value);
			}
		}

		/// <summary>
		/// If dictionary contains key, returns existing value. If dictionary does not contain key, returns parameter defaultValue.
		/// </summary>
		public static TV GetDefault<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV defaultValue) {
			if (dict.ContainsKey(key)) {
				return dict[key];
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Clears a list then adds new values
		/// </summary>
		public static void ClearAdd<T>(this List<T> list, IEnumerable<T> toAdd) {
			if (list == null) throw new NullReferenceException();
			list.Clear();
			list.AddRange(toAdd);
		}

	}

}