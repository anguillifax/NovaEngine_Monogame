using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System.Collections.Generic;

namespace Nova.Gui {

	public class PanelRebindGamepad : DrawableGameComponent {

		private readonly static SimpleButton ButtonRumbleToggle = new SimpleButton(Keys.F8);

		public PlayerIndex Index { get; }
		private InputSourceGamepad Source {
			get { return Index == PlayerIndex.One ? InputManager.SourceGamepad1 : InputManager.SourceGamepad2; }
		}

		private readonly List<Buttons> lastButtons = new List<Buttons>();
		private readonly List<Buttons> prevBoundButtons = new List<Buttons>();

		public int SelectionIndex { get; private set; }
		public VirtualGamepadButton Target { get; private set; }

		private int MaxButtons {
			get { return InputManager.SourceGamepad1.AllButtons.Count; }
		}

		public PanelRebindGamepad(Game game, PlayerIndex index) :
			base(game) {
			Index = index;
		}

		public override void Initialize() {
			Enabled = true;
			Visible = false;
			base.Initialize();
		}

		private void BeginEdit() {
			Target = Source.AllButtons[SelectionIndex];
			prevBoundButtons.ClearAdd(Target.ButtonList);
			foreach (var b in GlobalInputProperties.WhitelistedButtons) {
				if (GamePad.GetState(Index).IsButtonDown(b)) {
					lastButtons.Add(b);
				}
			}
		}

		public override void Update(GameTime t) {

			// Update visibility
			if (Index == PlayerIndex.One ? InputManager.RebindingPanelGamepad1.JustPressed : InputManager.RebindingPanelGamepad2.JustPressed) {
				Visible = !Visible;

				if (Visible) {
					BeginEdit();
				} else {
					InputManager.SaveBindings();
				}
			}

			if (Visible) {

				// Update rumble
				if (ButtonRumbleToggle.JustPressed) {
					Source.RumbleEnabled = !Source.RumbleEnabled;
				}


				// Update selection position
				int delta = 0;
				if (InputManager.Any.Vertical.RepeaterPos) {
					delta = -1;
				}
				if (InputManager.Any.Vertical.RepeaterNeg) {
					delta = +1;
				}
				if (delta != 0) {
					SelectionIndex = Calc.Loop(SelectionIndex + delta, 0, MaxButtons);
					BeginEdit();
				}


				// Do the actual rebinding
				if (Target.IsRebindable) {

					for (int i = 0; i < lastButtons.Count; i++) {
						if (GamePad.GetState(Index).IsButtonUp(lastButtons[i])) {
							lastButtons.RemoveAt(i);
							i--;
						}
					}

					foreach (var button in GlobalInputProperties.WhitelistedButtons) {
						if (GamePad.GetState(Index).IsButtonDown(button) && !lastButtons.Contains(button)) {
							var ret = Target.Rebind(button);
							lastButtons.Add(button);

							System.Console.WriteLine(ret);
						}
					}

					if (InputManager.Any.Clear.JustPressed) {
						Target.Unbind();
					}

					if (InputManager.Any.Back.JustPressed) {
						Target.ButtonList.ClearAdd(prevBoundButtons);
					}

				}

			}
		}

		public override void Draw(GameTime t) {

			if (!Visible) return;

			MDraw.Begin();

			var pos = new Vector2(20f, 20f);
			float firstSpace = 150;

			var title = string.Format("Rebind Panel: Gamepad {0}", Index == PlayerIndex.One ? "1" : "2");
			MDraw.Write(title, pos, Color.White);
			pos.X = Screen.Width * 0.2f;
			pos.Y += 40f;
			MDraw.Write("User", new Vector2(pos.X + firstSpace, pos.Y), Color.White);

			pos.Y += 60f;

			for (int i = 0; i < MaxButtons; i++) {

				bool active = SelectionIndex == i;
				var rowTarget = Source.AllButtons[i];
				Color color = rowTarget.IsRebindable ? (active ? Color.Yellow : Color.White) : (active ? new Color(150, 150, 80) : Color.Gray);

				Vector2 rowPos = pos.Copy();

				if (active) {
					rowPos.X += 5f;
				}

				MDraw.Write(rowTarget.Name, rowPos, color);

				rowPos.X += firstSpace;
				if (rowTarget.ButtonList.Count > 0) MDraw.Write(rowTarget.ButtonList.ToPrettyString(), rowPos, color);

				pos.Y += 40f;

			}

			MDraw.Write(string.Format("Rumble: {0} (press F8)", Source.RumbleEnabled ? "ON" : "OFF"), new Vector2(20f, Screen.Height - 40f), Color.DimGray);

			MDraw.SpriteBatch.End();

		}

	}

}