using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Nova.Input {

	public class VirtualGamepadButton : VirtualButtonBaseLogic {

		public readonly PlayerIndex Index;
		public bool Rebindable { get; private set; }

		public readonly List<Buttons> buttons;

		public VirtualGamepadButton(string name, PlayerIndex index) :
			base (name) {
			Index = index;
			Rebindable = true;
			buttons = new List<Buttons>();
		}

		public VirtualGamepadButton(string name, PlayerIndex index, params Buttons[] defaultButtons) :
			base(name) {
			Index = index;
			Rebindable = false;
			buttons = new List<Buttons>(defaultButtons);
		}

		protected override void Update() {

			lastValue = value;

			value = false;

			var s = GamePad.GetState(Index);
			foreach (var button in buttons) {
				if (s.IsButtonDown(button)) {
					value = true;
					break;
				}
			}

		}

		public RebindResult Rebind(Buttons newButton) {

			if (!Rebindable) {
				return RebindResult.NoOp;
			}

			if (GlobalInputProperties.IsButtonAllowed(newButton)) {

				if (buttons.Contains(newButton)) {
					buttons.Remove(newButton);
					return RebindResult.Removed;

				} else {
					if (buttons.Count >= GlobalInputProperties.MaxGamepadButtons) {
						// if too many buttons, remove from head
						buttons.RemoveRange(0, buttons.Count - GlobalInputProperties.MaxGamepadButtons + 1);
					}
					buttons.Add(newButton);
					return RebindResult.Added;
				}

			} else {
				return RebindResult.NoOp;
			}

		}

		public void Unbind() {
			if (Rebindable) {
				buttons.Clear();
			}
		}

	}

}