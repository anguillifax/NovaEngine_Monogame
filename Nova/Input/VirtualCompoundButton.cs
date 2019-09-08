using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	/// <summary>
	/// Packs multiple button outputs into a single button.
	/// </summary>
	public class VirtualCompoundButton : VirtualButton {

		private readonly List<VirtualButton> SubButtons;

		public VirtualCompoundButton(string name, params VirtualButton[] buttons) :
			base(name) {
			SubButtons = new List<VirtualButton>(buttons);
		}

		/// <summary>
		/// Clears all previous buttons and sets new buttons.
		/// </summary>
		public void SetNew(IEnumerable<VirtualButton> buttons) {
			SubButtons.Clear();
			SubButtons.AddRange(buttons);
		}

		/// <summary>
		/// Clears all buttons.
		/// </summary>
		public void Clear() {
			SubButtons.Clear();
		}

		#region Accessors

		public override bool Pressed {
			get {
				foreach (var item in SubButtons) {
					if (item.Pressed) return true;
				}
				return false;
			}
		}

		public override bool JustPressed {
			get {
				foreach (var item in SubButtons) {
					if (item.JustPressed) return true;
				}
				return false;
			}
		}

		public override bool Released {
			get {
				foreach (var item in SubButtons) {
					if (item.Pressed) return false;
				}
				return true;
			}
		}

		public override bool JustReleased {
			get {
				foreach (var item in SubButtons) {
					if (item.JustReleased) return Released;
				}
				return false;
			}
		}

		#endregion

	}

}