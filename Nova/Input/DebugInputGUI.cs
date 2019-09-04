using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Nova.Input {

	public static class DebugInputGUI {

		public static bool IsOpen { get; set; }

		public static int SelectionIndex { get; private set; }

		public static void Update() {

			if (InputManager.RebindingPanel.JustPressed) {
				IsOpen = !IsOpen;
			}

			if (IsOpen) {

				bool isMovementAxisSelected = InputManager.AllButtons[SelectionIndex].IsVirtualAxisMovementKey;

				int delta = 0;
				if (InputManager.Vertical.PressedPositive) {
					delta = -1;
				}
				if (InputManager.Vertical.PressedNegative) {
					delta = +1;
				}
				if (delta != 0) {
					SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, InputManager.AllButtons.Count);
				}

				RebindingManager.Target = InputManager.AllButtons[SelectionIndex];
				RebindingManager.Update();

			}

		}

		static SpriteBatch b;

		public static void Draw() {

			if (!IsOpen) return;

			b = DrawManager.SpriteBatch;
			b.Begin();

			var pos = new Vector2(Screen.Width * 0.1f, 20f);
			float x2 = pos.X + 300;
			float x3 = x2 + 200;
			float x4 = x3 + 200;

			Write("Rebind Panel", pos, Color.White);
			Write("KB Custom", new Vector2(x2, pos.Y), Color.White);
			Write("KB Default", new Vector2(x3, pos.Y), Color.White);
			Write("Gamepad", new Vector2(x4, pos.Y), Color.White);
			pos.Y += 60;

			for (int i = 0; i < InputManager.AllButtons.Count; i++) {
				var cur = InputManager.AllButtons[i];
				Color color = SelectionIndex == i ? Color.Yellow : Color.White;

				Write(cur.Name, pos, color);

				var rowPos = new Vector2(x2, pos.Y);
				if (cur.keyboard.HasKey) Write(cur.keyboard.Key.ToString(), rowPos, color);

				rowPos.X = x3;
				if (cur.keyboardDefault.HasKey) Write(cur.keyboardDefault.Key.ToString(), rowPos, color);

				rowPos.X = x4;
				if (cur.gamepad.HasButton) Write(PrintFormatter.ListToString(cur.gamepad.buttons), rowPos, color);

				pos.Y += 50f;
			}

			b.End();
		}

		private static void Write(string text, Vector2 pos, Color color) {
			b.DrawString(Engine.DefaultFont, text, pos, color);
		}

	}

}