using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public class PanelRebindInputSources : DrawableGameComponent {

		public int SelectionIndex { get; private set; }

		private InputSourceLayoutData Toggles {
			get { return BindingManager.CurrentBindings.InputSourceLayout; }
		}
		// [0] true: KB is Player 2
		// [1] true: GP1 is Player 2 && GP2 is Player 1

		public PanelRebindInputSources(Game game) :
			base(game) {
		}

		public override void Initialize() {
			Enabled = true;
			Visible = false;
			base.Initialize();
		}

		public override void Update(GameTime gameTime) {

			// Update visibility
			if (InputManager.RebindingPanelInputSources.JustPressed) {
				Visible = !Visible;

				if (!Visible) {
					BindingManager.Save();
				}
			}

			if (Visible) {

				int delta = 0;
				if (InputManager.Any.Vertical.RepeaterPos) {
					delta = -1;
				}
				if (InputManager.Any.Vertical.RepeaterNeg) {
					delta = +1;
				}
				if (delta != 0) {
					SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, 2);
				}

				if (InputManager.Any.Enter.JustPressed) {
					switch (SelectionIndex) {
						case 0:
							BindingManager.CurrentBindings.InputSourceLayout.keyboardIsPlayer2 = !Toggles.keyboardIsPlayer2;
							break;
						case 1:
							BindingManager.CurrentBindings.InputSourceLayout.gamepad1IsPlayer2 = !Toggles.gamepad1IsPlayer2;
							break;

						default:
							break;
					}

					InputSourceLayoutManager.ApplyFromCurrent();
				}

			}

		}

		public override void Draw(GameTime t) {

			if (!Visible) return;

			MDraw.Begin();

			MDraw.Write("Rebind Input Source Layout", new Vector2(20f, 20f), Color.White);

			float leftPos = 50f;
			float x1 = 400f;
			float x2 = 600f;
			float shift = 5f;
			var pos = new Vector2(0, 90f);
			Color color;

			pos.X = x1;
			MDraw.Write("Player 1", pos, Color.White);

			pos.X = x2;
			MDraw.Write("Player 2", pos, Color.White);

			pos.X = leftPos;
			pos.Y += 80f;

			if (SelectionIndex == 0) pos.X += shift;
			color = SelectionIndex == 0 ? Color.Yellow : Color.White;
			MDraw.Write("Swap Keyboard", pos, color);

			pos.X = Toggles.keyboardIsPlayer2 ? x2 : x1;
			MDraw.Write("Keyboard", pos, color);

			pos.X = leftPos;
			pos.Y += 60f;

			color = SelectionIndex == 1 ? Color.Yellow : Color.White;
			if (SelectionIndex == 1) pos.X += shift;
			MDraw.Write("Swap Gamepad", pos, color);

			pos.X = Toggles.gamepad1IsPlayer2 ? x2 : x1;
			MDraw.Write("Gamepad1", pos, color);

			pos.X = Toggles.gamepad1IsPlayer2 ? x1 : x2;
			MDraw.Write("Gamepad2", pos, color);

			MDraw.End();
		}

	}

}