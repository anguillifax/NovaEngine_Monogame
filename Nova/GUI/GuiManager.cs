using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui {

	public static class GuiManager {

		public static GuiStateMachine StateManager { get; private set; }

		internal static void Init() {
			StateManager = new GuiStateMachine(new TestGuiElement(), new TestGuiElement2(), new TestSlowPanel());
			StateManager.ClearAndSetCurrent(StateManager.AllElements[0]);
		}

		internal static void Update() {
			StateManager.Update();
		}

		internal static void Draw() {
			MDraw.Begin();
			StateManager.Draw();
			MDraw.End();
		}

	}

}