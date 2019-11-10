using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui {

	public static class GuiManager {

		public static RootElement Root { get; private set; }

		public static GuiStateMachine StateManager { get; private set; }

		internal static void Init() {
			//StateManager = new GuiStateMachine(new TestGuiElement(), new TestGuiElement2(), new TestSlowPanel());
			//StateManager.ClearAndSetCurrent(StateManager.AllElements[0]);

			Root = new RootElement();

			var halfLeft = new RegionElement(
				(x) => new FloatRect(x.Position, new Vector2(x.Size.X * 0.5f, x.Size.Y)));

			var miniRight = new RegionElement(
				(x) => new FloatRect(x.Position.X + x.Size.X / 2, x.Position.Y + x.Size.Y / 2, x.Size.X / 4 + 40, x.Size.Y / 4 + 40));

			ElementUtil.Adopt(Root, halfLeft, miniRight);

		}

		internal static void Update() {
			Root.Resize();
			Root.Update();
			//Console.WriteLine(Screen.Size);
			//Console.WriteLine();
			//StateManager.Update();
		}

		internal static void Draw() {
			MDraw.Begin();
			//Root.Draw();
			//StateManager.Draw();
			MDraw.End();

			MDraw.Test();
		}

	}

}