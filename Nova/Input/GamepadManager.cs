using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public static class GamepadManager {

		public static int GamepadCount {
			get {
				int count = 0;
				for (int i = 0; i < 2; i++) {
					if (GamePad.GetState(i).IsConnected) count++;
				}
				return count;
			}
		}

	}

}