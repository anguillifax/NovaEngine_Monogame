using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public abstract class VirtualInput {

		public readonly bool GamepadRebindable;

		protected VirtualInput(bool gamepadRebindable) {
			GamepadRebindable = gamepadRebindable;
			InputManager.InputUpdate += Update;
			InputBindingsManager.LoadBindings += OnLoadBindings;
			InputBindingsManager.SaveBindings += OnSaveBindings;
		}

		protected abstract void Update();
		protected abstract void OnSaveBindings();
		protected abstract void OnLoadBindings();

	}

}