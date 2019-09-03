using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Input {

	[Serializable]
	public class BindingData {
		public Keys Keyboard;
		public List<Buttons> Gamepad; // this list is never null

		public BindingData() {
			Keyboard = Keys.None;
			Gamepad = new List<Buttons>();
		}

		public BindingData(Keys keyboard, params Buttons[] gamepad) {
			Keyboard = keyboard;
			Gamepad = new List<Buttons>(gamepad);
		}

		public BindingData(Keys keyboard, List<Buttons> gamepad) {
			Keyboard = keyboard;
			if (gamepad == null) {
				Gamepad = new List<Buttons>();
			} else {
				Gamepad = gamepad;
			}
		}

		public BindingData(Keys keyboard) {
			Keyboard = keyboard;
			Gamepad = new List<Buttons>();
		}
	}

	/// <summary>
	/// Stores the mapping between virtual inputs and their bindings
	/// </summary>
	[Serializable]
	public class Bindings {
		public readonly Dictionary<string, BindingData> Map;

		public BindingData this[string key] {
			get {
				if (Map.ContainsKey(key)) {
					return Map[key];
				} else {
					// Saved binding configuration did not contain key.
					// Upon receiving null, VirtualInput will unbind currently bound keys.
					return null;
				}
			}
			set {
				if (Map.ContainsKey(key)) {
					Map[key] = value;
				} else {
					Map.Add(key, value);
				}
			}
		}

		public Bindings() {
			Map = new Dictionary<string, BindingData>();
		}

		public Bindings(Bindings toCopy) {
			Map = new Dictionary<string, BindingData>(toCopy.Map);
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("== Editable Bindings ==\n");
			foreach (var item in Map) {
				sb.AppendLine(item.Key);
				sb.AppendLine("   " + item.Value.Keyboard);
				sb.AppendLine("   " + PrintFormatter.ListToString(item.Value.Gamepad));
				sb.AppendLine();
			}
			return sb.ToString();
		}

	}

}