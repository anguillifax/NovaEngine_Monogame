using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;

namespace Nova.Gui {

	public class PanelRebindKeyboard : DrawableGameComponent {

		private static readonly SimpleButton ButtonHardcodedEnter = new SimpleButton(Keys.Enter);
		private static readonly SimpleButton ButtonHardcodedBack = new SimpleButton(Keys.Escape);
		private static readonly SimpleButton ButtonHardcodedClear = new SimpleButton(Keys.Back);

		public int SelectionIndex { get; private set; }
		public bool IsEditingKeyboard { get; private set; }

		private int MaxButtons {
			get { return MInput.SourceKeyboard.AllButtons.Count; }
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

			UpdateVisibility();

			if (Visible) {

				UpdateEditMode();
				UpdateSelectedValue();
				UpdateRebind();

			}
		}

		private void UpdateVisibility() {
			if (MInput.RebindingPanelKeyboard.JustPressed) {
				Visible = !Visible;
				IsEditingKeyboard = false;

				if (!Visible) {
					MInput.SaveBindings();
				}
			}
		}

		private void UpdateEditMode() {
			if (!IsEditingKeyboard && MInput.Any.Enter.JustPressed) {
				IsEditingKeyboard = true;
				KeyboardRebindingManager.BeginRebinding(MInput.SourceKeyboard.AllButtons[SelectionIndex]);
				System.Console.WriteLine("edit");

			} else if (IsEditingKeyboard && ButtonHardcodedEnter.JustPressed) {
				IsEditingKeyboard = false;
				System.Console.WriteLine("stop");
			}
		}

		private void UpdateSelectedValue() {
			if (IsEditingKeyboard) return;

			int delta = 0;
			if (MInput.Any.Vertical.RepeaterPos) {
				delta = -1;
			}
			if (MInput.Any.Vertical.RepeaterNeg) {
				delta = +1;
			}
			if (delta != 0) {
				SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, MaxButtons);
			}
		}

		private void UpdateRebind() {
			if (IsEditingKeyboard) {

				KeyboardRebindingManager.Update();
				if (ButtonHardcodedClear.JustPressed) {
					KeyboardRebindingManager.Unbind();
				}
				if (ButtonHardcodedBack.JustPressed) {
					KeyboardRebindingManager.CancelOperation();
					IsEditingKeyboard = false;
					System.Console.WriteLine("stop");
				}

			} else {
				if (MInput.Any.Clear.JustPressed) {
					KeyboardRebindingManager.Unbind();
				}
			}
		}

		public override void Draw(GameTime t) {

			if (!Visible) return;

			MDraw.Begin();

			var pos = new Vector2(Screen.Width * 0.1f, 20f);
			float firstSpace = 150;
			float inputSpace = 200;

			MDraw.Write("Rebind Panel: Keyboard", pos, Color.White);
			pos.X = Screen.Width * 0.2f;
			pos.Y += 80f;
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

				MDraw.Write(MInput.SourceKeyboard.AllButtons[i].Name, rowPos, color);

				rowPos.X += firstSpace;
				var kb = MInput.SourceKeyboard.AllButtons[i];
				if (kb.UserKey != null) MDraw.Write(kb.UserKey.ToString(), rowPos, color);

				rowPos.X += inputSpace;
				if (kb.HardcodedKey != null) MDraw.Write(kb.HardcodedKey.ToString(), rowPos, color);

				pos.Y += 50f;

			}

			MDraw.SpriteBatch.End();
		}

	}

}