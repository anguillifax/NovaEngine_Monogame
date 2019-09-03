using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Project.Input {

	public abstract class VirtualInput {

		protected readonly bool GamepadReconfigurable;

		protected VirtualInput(bool gamepadReconfigurable) {
			GamepadReconfigurable = gamepadReconfigurable;
			InputBindingsManager.LoadBindings += OnLoadBindings;
			InputBindingsManager.SaveBindings += OnSaveBindings;
		}

		protected abstract void OnSaveBindings();
		protected abstract void OnLoadBindings();

	}

}