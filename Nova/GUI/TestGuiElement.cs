using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public class TestGuiElement : GuiElement {

		protected override bool NextElementSelector(GuiElement element) {
			if (InputManager.Any.Attack.JustPressed) {

				return element is TestGuiElement2;

			}

			return false;
		}

		public override void Resize() {
			Bounds.SetBounds(Screen.Height / 2, 0, 0, Screen.Width / 2);
			Console.WriteLine($"[1] resized to {Bounds}");
		}

		public override void Draw() {
			MDraw.DrawBoxScreen(Bounds.TopLeft, Bounds.BottomRight, Color.Lime);
		}

	}

}