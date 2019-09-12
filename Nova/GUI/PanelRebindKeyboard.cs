using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System.Collections.Generic;

namespace Nova.Gui {

	public class PanelRebindKeyboard : DrawableGameComponent {

		private static readonly SimpleButton ButtonHardcodedEnter = new SimpleButton(Keys.Enter);
		private static readonly SimpleButton ButtonHardcodedBack = new SimpleButton(Keys.Escape);
		private static readonly SimpleButton ButtonHardcodedClear = new SimpleButton(Keys.Back);

		private Keys? prevBoundKey;
		private readonly List<Keys> lastKeys = new List<Keys>();

		public int SelectionIndex { get; private set; }
		public VirtualKeyboardButton Target { get; private set; }
		public bool IsEditingKeyboard { get; private set; }


		private int MaxButtons {
			get { return InputManager.SourceKeyboard.AllButtons.Count; }
		}

		public PanelRebindKeyboard(Game game) :
			base(game) {
		}

		public override void Initialize() {
			Enabled = true;
			Visible = false;
			base.Initialize();
		}

		public override void Update(GameTime t) {

			// Update visibility
			if (InputManager.RebindingPanelKeyboard.JustPressed) {
				Visible = !Visible;
				IsEditingKeyboard = false;

				if (!Visible) {
					InputManager.SaveBindings();
				}
			}

			if (Visible) {

				// Update selected value if not in edit mode
				if (!IsEditingKeyboard) {

					int delta = 0;
					if (InputManager.Any.Vertical.RepeaterPos) {
						delta = -1;
					}
					if (InputManager.Any.Vertical.RepeaterNeg) {
						delta = +1;
					}
					if (delta != 0) {
						SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, MaxButtons);
						Target = InputManager.SourceKeyboard.AllButtons[SelectionIndex];
					}

				}


				// Update edit mode
				if (!IsEditingKeyboard && InputManager.Any.Enter.JustPressed) {
					IsEditingKeyboard = true;

					lastKeys.ClearAdd(Keyboard.GetState().GetPressedKeys());
					prevBoundKey = Target.UserKey;

					System.Console.WriteLine("edit");

				} else if (IsEditingKeyboard && (ButtonHardcodedEnter.JustPressed || InputManager.SourceGamepad1.Enter.JustPressed || InputManager.SourceGamepad2.Enter.JustPressed)) {
					IsEditingKeyboard = false;

					System.Console.WriteLine("stop");
				}


				// Do the actual rebinding
				if (IsEditingKeyboard) {

					var s = Keyboard.GetState();

					for (int i = 0; i < lastKeys.Count; i++) {
						if (s.IsKeyUp(lastKeys[i])) {
							lastKeys.RemoveAt(i);
							i--;
						}
					}

					foreach (var key in s.GetPressedKeys()) {
						if (!lastKeys.Contains(key)) {
							var ret = Target.Rebind(key);
							lastKeys.Add(key);

							System.Console.WriteLine(ret);
						}
					}

					// Clear
					if (ButtonHardcodedClear.JustPressed || InputManager.SourceGamepad1.Clear.JustPressed || InputManager.SourceGamepad2.Clear.JustPressed) {
						Target.Unbind();
					}

					// Cancel and revert
					if (ButtonHardcodedBack.JustPressed || InputManager.SourceGamepad1.Back.JustPressed || InputManager.SourceGamepad2.Back.JustPressed) {

						Target.UserKey = prevBoundKey;

						IsEditingKeyboard = false;

						System.Console.WriteLine("stop");
					}

				} else {
					// Clear
					if (InputManager.Any.Clear.JustPressed) {
						Target.Unbind();
					}
				}

			}
		}

		public override void Draw(GameTime t) {

			if (!Visible) return;

			MDraw.Begin();

			var pos = new Vector2(20f, 20f);
			float firstSpace = 150;
			float inputSpace = 200;

			MDraw.Write("Rebind Panel: Keyboard", pos, Color.White);
			pos.X = Screen.Width * 0.2f;
			pos.Y += 40f;
			MDraw.Write("User", new Vector2(pos.X + firstSpace, pos.Y), Color.White);
			MDraw.Write("Default", new Vector2(pos.X + firstSpace + inputSpace, pos.Y), Color.White);

			pos.Y += 60f;

			for (int i = 0; i < MaxButtons; i++) {

				bool active = SelectionIndex == i;
				Color color = active ? (IsEditingKeyboard ? Color.Lime : Color.Yellow) : (IsEditingKeyboard ? Color.Gray : Color.White);

				Vector2 rowPos = pos.Copy();

				if (active) {
					rowPos.X += 5f;
				}

				MDraw.Write(InputManager.SourceKeyboard.AllButtons[i].Name, rowPos, color);

				rowPos.X += firstSpace;
				var kb = InputManager.SourceKeyboard.AllButtons[i];
				if (kb.UserKey != null) MDraw.Write(kb.UserKey.ToString(), rowPos, color);

				rowPos.X += inputSpace;
				if (kb.HardcodedKey != null) MDraw.Write(kb.HardcodedKey.ToString(), rowPos, color);

				pos.Y += 40f;

			}

			MDraw.SpriteBatch.End();
		}

	}

}