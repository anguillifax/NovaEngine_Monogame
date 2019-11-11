using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nova.Util;
using System;
using System.Collections.Generic;

namespace Nova.Input {

	public class VirtualGamepadButton : VirtualButtonBaseLogic {

		public PlayerIndex Index { get; }
		public bool IsRebindable { get; }

		public readonly List<Buttons> ButtonList;

		private GamepadBindingData CurrentBindingData {
			get {
				return BindingManager.CurrentBindings.GetGamepad(Index);
			}
		}

		/// <summary>
		/// Creates a new VirtualGamepadButton that can be rebound.
		/// </summary>
		public VirtualGamepadButton(string name, PlayerIndex index) :
			base(name) {
			Index = index;
			IsRebindable = true;
			ButtonList = new List<Buttons>();
		}

		/// <summary>
		/// Creates a new VirtualGamepadButton with hardcoded controls.
		/// </summary>
		public VirtualGamepadButton(string name, PlayerIndex index, params Buttons[] defaultButtons) :
			base(name) {
			Index = index;
			IsRebindable = false;
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
			if (IsRebindable) {
				ButtonList.ClearAdd(CurrentBindingData[Name]);
			}
		}

		protected override void OnSaveBinding() {
			if (IsRebindable) {
				CurrentBindingData[Name] = ButtonList;
			}
		}

		public RebindResult Rebind(Buttons newButton) {

			if (!IsRebindable) {
				return RebindResult.NotAllowed;
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
				return RebindResult.NotAllowed;
			}

		}

		public void Unbind() {
			if (IsRebindable) {
				ButtonList.Clear();
			}
		}

	}

}