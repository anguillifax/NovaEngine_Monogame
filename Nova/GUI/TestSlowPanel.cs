using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public class TestSlowPanel : GuiElement {

		private readonly SimpleTimer exitTimer = new SimpleTimer(3f);

		protected override bool NextElementSelector(GuiElement element) {
			if (InputManager.Any.Attack.JustPressed) {

				return element is TestGuiElement;

			}

			return false;
		}

		public override void Update() {
			exitTimer.Update();
		}

		public override void OnExit() {
			base.OnExit();
			Console.WriteLine("Beginning long timer...");
			exitTimer.Reset();
		}

		public override bool IsExitTransitionFinished() {
			if (exitTimer.Done) {
				Console.WriteLine("Exit finally complete");
				return true;
			}
			return false;
		}

		public override void Resize() {
			Bounds.SetBounds(Screen.Height / 1.6f, 0, 0, Screen.Width / 1.6f);
			Console.WriteLine($"[SLOW] resized to {Bounds}");
		}

		public override void Draw() {
			MDraw.DrawBoxScreen(Bounds.TopLeft, Bounds.BottomRight, IsExiting ? Color.CadetBlue.Multiply(0.5f) : Color.CadetBlue);
		}

	}

}