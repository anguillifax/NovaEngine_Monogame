using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
			SubButtons.ClearAdd(buttons);
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
				return SubButtons.Any((x) => x.Pressed);
			}
		}

		public override bool JustPressed {
			get {
				int jpCount = SubButtons.Count((x) => x.JustPressed);
				if (jpCount == 0) {
					return false;
				} else {
					int pCount = SubButtons.Count((x) => x.Pressed);
					return pCount == jpCount;
				}
			}
		}

		public override bool Released {
			get {
				return SubButtons.All((x) => x.Released);
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

	/// <summary>
	/// Uses a Linq query to automatically retrieve new buttons.
	/// </summary>
	public class AutoVirtualCompoundButton : VirtualCompoundButton {

		/// <summary>
		/// Defines what VirtualButton to retrieve from an InputSource.
		/// </summary>
		private readonly Func<InputSource, VirtualButton> selector;

		public AutoVirtualCompoundButton(string name, Func<InputSource, VirtualButton> func, params VirtualButton[] buttons) :
			base(name, buttons) {
			selector = func;
		}

		public void UpdateButtons(List<InputSource> sources) {
			SetNew(sources.Select(selector));
		}

	}

}