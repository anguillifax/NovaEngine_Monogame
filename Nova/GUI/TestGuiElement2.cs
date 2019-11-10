using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	//public class TestGuiElement2 : GuiElement {

	//	private readonly SimpleTimer exitTimer = new SimpleTimer(0.5f);

	//	protected override bool NextElementSelector(GuiElement element) {
	//		if (InputManager.Any.Attack.JustPressed) {

	//			return element is TestSlowPanel;

	//		}

	//		return false;
	//	}

	//	public override void Update() {
	//		exitTimer.Update();
	//	}

	//	public override void OnExit() {
	//		base.OnExit();
	//		Console.WriteLine("Beginning timer...");
	//		exitTimer.Reset();
	//	}

	//	public override bool IsExitTransitionFinished() {
	//		if (exitTimer.Done) {
	//			Console.WriteLine("Exit complete");
	//			return true;
	//		}
	//		return false;
	//	}

	//	public override void Resize() {
	//		Bounds.SetBounds(Screen.Height / 1.2f, 0, 0, Screen.Width / 1.2f);
	//		Console.WriteLine($"[2] resized to {Bounds}");
	//	}

	//	public override void Draw() {
	//		MDraw.DrawBoxScreen(Bounds.TopLeft, Bounds.BottomRight, IsExiting ? Color.DarkGreen : Color.LimeGreen);
	//	}

	//}

}