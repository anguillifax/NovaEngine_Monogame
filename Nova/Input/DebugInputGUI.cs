using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Nova.Input {

	public static class DebugInputGUI {

		public static bool IsOpen { get; set; }

		public static int SelectionIndex { get; private set; }
		public static bool IsEditingKeyboard { get; private set; }

		public static void Update() {

			if (InputManager.RebindingPanel.JustPressed) {
				IsOpen = !IsOpen;
				IsEditingKeyboard = false;

				if (!IsOpen) {
					InputManager.SaveBindings();
				}
			}

			if (IsOpen) {

				if (!IsEditingKeyboard && (InputManager.Enter.SourceKeyboard.JustPressed || InputManager.Enter.SourceKeyboardDefault.JustPressed)) {
					IsEditingKeyboard = true;
					RebindingManager.RegisterAlreadyPressedKeys(Keyboard.GetState().GetPressedKeys());

				} else if (IsEditingKeyboard && InputManager.Enter.SourceKeyboardDefault.JustPressed) {
					IsEditingKeyboard = false;
				}

				if (!IsEditingKeyboard) {
					int delta = 0;
					if (InputManager.Vertical.RepeaterPos) {
						delta = -1;
					}
					if (InputManager.Vertical.RepeaterNeg) {
						delta = +1;
					}
					if (delta != 0) {
						SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, InputManager.AllButtons.Count);
					}
				}

				RebindingManager.Target = InputManager.AllButtons[SelectionIndex];
				if (IsEditingKeyboard) {
					RebindingManager.SearchKeyboard();
					if (InputManager.Clear.SourceKeyboardDefault.JustPressed) {
						RebindingManager.Target.SourceKeyboard.Unbind();
					}
				} else {
					if (InputManager.Clear.SourceKeyboard.JustPressed || InputManager.Clear.SourceKeyboardDefault.JustPressed) {
						RebindingManager.Target.SourceKeyboard.Unbind();
					}
				}

				RebindingManager.SearchGamepad();
				if (InputManager.Clear.SourceGamepad.JustPressed && RebindingManager.Target.GamepadRebindable) {
					RebindingManager.Target.SourceGamepad.UnbindAll();
				}

			}

		}

		static SpriteBatch b;

		public static void Draw() {

			if (!IsOpen) return;

			b = DrawManager.SpriteBatch;
			b.Begin();

			var pos = new Vector2(Screen.Width * 0.1f, 20f);
			float firstSpace = 300;
			float inputSpace = 200;

			Write("Rebind Panel", pos, Color.White);
			Write("KB Custom", new Vector2(pos.X + firstSpace, pos.Y), Color.White);
			Write("KB Default", new Vector2(pos.X + firstSpace + inputSpace, pos.Y), Color.White);
			Write("Gamepad", new Vector2(pos.X + firstSpace + 2 * inputSpace, pos.Y), Color.White);
			pos.Y += 60;

			for (int i = 0; i < InputManager.AllButtons.Count; i++) {
				var cur = InputManager.AllButtons[i];
				bool active = SelectionIndex == i;
				Color color = active ? (IsEditingKeyboard ? Color.Lime : Color.Yellow) : (IsEditingKeyboard ? Color.Gray : Color.White);

				Vector2 rowPos = pos.Copy();

				if (active) {
					rowPos.X += 5f;
				}

				Write(cur.Name, rowPos, color);

				rowPos.X += firstSpace;
				if (cur.SourceKeyboard.HasKey) Write(cur.SourceKeyboard.Key.ToString(), rowPos, color);

				rowPos.X += inputSpace;
				if (cur.SourceKeyboardDefault.HasKey) Write(cur.SourceKeyboardDefault.Key.ToString(), rowPos, color);

				rowPos.X += inputSpace;
				if (cur.SourceGamepad.HasButton) Write(PrintFormatter.ListToString(cur.SourceGamepad.buttons), rowPos, color);

				pos.Y += 50f;
			}

			b.End();
		}

		private static void Write(string text, Vector2 pos, Color color) {
			b.DrawString(Engine.DefaultFont, text, pos, color);
		}

	}

}