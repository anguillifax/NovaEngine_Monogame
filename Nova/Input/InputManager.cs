using Microsoft.Xna.Framework.Input;
using Project.Input;
using System;

namespace Project {

	public static class InputManager {

		public static event Action InputUpdate;

		public static void Update() {
			InputUpdate?.Invoke();
		}

	}

	

}