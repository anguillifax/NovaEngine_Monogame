using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public static class RebindingManagerKeyboard {

		private readonly static List<Keys> lastKeys = new List<Keys>();

		public static VirtualKeyboardButton Target { get; set; }

		public static void Update() {
			var s = Keyboard.GetState();

			for (int i = 0; i < lastKeys.Count; i++) {
				if (s.IsKeyUp(lastKeys[i])) {
					lastKeys.RemoveAt(i);
					i--;
				}
			}

			foreach (var key in s.GetPressedKeys()) {
				if (!lastKeys.Contains(key)) {
					var ret = Target.Rebind(key);
					lastKeys.Add(key);
				}
			}

		}

		public static void Unbind() {
			Target.Unbind();
		}

		public static void RegisterAlreadyPressedKeys(Keys[] keys) {
			lastKeys.AddRange(keys);
		}

	}

}