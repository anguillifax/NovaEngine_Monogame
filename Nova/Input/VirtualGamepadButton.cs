using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public class VirtualGamepadButton : VirtualButtonBaseLogic {

		public readonly PlayerIndex Index;
		public bool Rebindable { get; private set; }

		public readonly List<Buttons> ButtonList;

		private GamepadBindingData CurrentBindingData {
			get {
				switch (Index) {
					case PlayerIndex.One:
						return BindingManager.CurrentBindings.Gamepad1;
					case PlayerIndex.Two:
						return BindingManager.CurrentBindings.Gamepad2;
					default:
						throw new Exception("Player index is not set to 1 or 2!");
				}
			}
		}

		/// <summary>
		/// Creates a new VirtualGamepadButton that can be rebound.
		/// </summary>
		public VirtualGamepadButton(string name, PlayerIndex index) :
			base (name) {
			Index = index;
			Rebindable = true;
			ButtonList = new List<Buttons>();
		}

		/// <summary>
		/// Creates a new VirtualGamepadButton with hardcoded controls.
		/// </summary>
		public VirtualGamepadButton(string name, PlayerIndex index, params Buttons[] defaultButtons) :
			base(name) {
			Index = index;
			Rebindable = false;
			ButtonList = new List<Buttons>(defaultButtons);
		}

		protected override void Update() {

			lastValue = value;

			value = false;

			var s = GamePad.GetState(Index);
			foreach (var button in ButtonList) {
				if (s.IsButtonDown(button)) {
					value = true;
					break;
				}
			}

		}

		protected override void OnLoadBinding() {
			if (Rebindable) {
				ButtonList.Clear();
				ButtonList.AddRange(CurrentBindingData[Name]);
			}
		}

		protected override void OnSaveBinding() {
			CurrentBindingData[Name] = ButtonList;
		}

		public RebindResult Rebind(Buttons newButton) {

			if (!Rebindable) {
				return RebindResult.NoOp;
			}

			if (GlobalInputProperties.IsButtonAllowed(newButton)) {

				if (ButtonList.Contains(newButton)) {
					ButtonList.Remove(newButton);
					return RebindResult.Removed;

				} else {
					if (ButtonList.Count >= GlobalInputProperties.MaxGamepadButtons) {
						// if too many buttons, remove from head
						ButtonList.RemoveRange(0, ButtonList.Count - GlobalInputProperties.MaxGamepadButtons + 1);
					}
					ButtonList.Add(newButton);
					return RebindResult.Added;
				}

			} else {
				return RebindResult.NoOp;
			}

		}

		public void Unbind() {
			if (Rebindable) {
				ButtonList.Clear();
			}
		}

	}

}