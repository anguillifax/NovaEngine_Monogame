using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public static class RebindingManager {

		private readonly static List<Keys> lastKeys = new List<Keys>();
		private readonly static List<Buttons> lastButtons = new List<Buttons>();

		//var s = Keyboard.GetState();

		//for (int i = 0; i < lastKeys.Count; i++) {
		//	if (s.IsKeyUp(lastKeys[i])) {
		//		lastKeys.RemoveAt(i);
		//		i--;
		//	}
		//}

	}

}