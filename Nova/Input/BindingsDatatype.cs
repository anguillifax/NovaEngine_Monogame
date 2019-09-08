using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nova.Input {

	[Serializable]
	public struct CombinedBindingData {
		public KeyboardBindingData Keyboard;
		public GamepadBindingData Gamepad1;
		public GamepadBindingData Gamepad2;

		public CombinedBindingData(KeyboardBindingData keyboard, GamepadBindingData gamepad1, GamepadBindingData gamepad2) {
			Keyboard = keyboard;
			Gamepad1 = gamepad1;
			Gamepad2 = gamepad2;
		}

		public CombinedBindingData(CombinedBindingData other) {
			Keyboard = new KeyboardBindingData(other.Keyboard);
			Gamepad1 = new GamepadBindingData(other.Gamepad1);
			Gamepad2 = new GamepadBindingData(other.Gamepad2);
		}

	}

	[Serializable]
	public class KeyboardBindingData {
		protected Dictionary<string, Keys?> Mapping;

		public Keys? this[string key] {
			get {
				return Mapping.GetDefault(key, null);
			}
			set {
				Mapping.AddUpdate(key, value);
			}
		}

		/// <summary>
		/// Bypasses checks and directly adds a new key to the mapping. Use only during initialization.
		/// </summary>
		public void Add(string key, Keys value) {
			Mapping.Add(key, value);
		}

		public KeyboardBindingData() {
			Mapping = new Dictionary<string, Keys?>();
		}

		public KeyboardBindingData(KeyboardBindingData other) {
			Mapping = new Dictionary<string, Keys?>(other.Mapping);
		}
	}

	[Serializable]
	public class GamepadBindingData {
		protected Dictionary<string, List<Buttons>> Mapping; // List is never null.
		public PlayerIndex Index;
		public bool RumbleEnabled;

		public List<Buttons> this[string key] {
			get {
				return Mapping.GetDefault(key, new List<Buttons>());
			}
			set {
				if (Mapping.ContainsKey(key)) {
					if (Mapping[key] == null) {
						Mapping[key] = new List<Buttons>(value);
					} else {
						Mapping[key].Clear();
						Mapping[key].AddRange(value);
					}
				} else {
					if (value == null) {
						Mapping.Add(key, new List<Buttons>());
					} else {
						Mapping.Add(key, new List<Buttons>(value));
					}
				}
			}
		}

		/// <summary>
		/// Bypasses checks and directly adds a new button to the mapping. Use only during initialization.
		/// </summary>
		public void Add(string key, params Buttons[] btns) {
			Mapping.Add(key, new List<Buttons>(btns));
		}

		public GamepadBindingData(PlayerIndex index) {
			Mapping = new Dictionary<string, List<Buttons>>();
			Index = index;
		}

		public GamepadBindingData(GamepadBindingData other) {
			Mapping = new Dictionary<string, List<Buttons>>(other.Mapping);
		}

		public GamepadBindingData(GamepadBindingData other, PlayerIndex newIndex) {
			Mapping = new Dictionary<string, List<Buttons>>(other.Mapping);
			Index = newIndex;
		}

	}

}