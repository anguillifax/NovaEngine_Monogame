using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nova.Input {

	[Serializable]
	public struct CombinedBindingData {
		public KeyboardBindingData Keyboard;
		public GamepadBindingData Gamepad1;
		public GamepadBindingData Gamepad2;
		public InputSourceLayoutData InputSourceLayout;

		public CombinedBindingData(KeyboardBindingData keyboard, GamepadBindingData gamepad1, GamepadBindingData gamepad2) {
			Keyboard = keyboard;
			Gamepad1 = gamepad1;
			Gamepad2 = gamepad2;
			InputSourceLayout = new InputSourceLayoutData();
		}

		public CombinedBindingData(CombinedBindingData other) {
			Keyboard = new KeyboardBindingData(other.Keyboard);
			Gamepad1 = new GamepadBindingData(other.Gamepad1);
			Gamepad2 = new GamepadBindingData(other.Gamepad2);
			InputSourceLayout = new InputSourceLayoutData(other.InputSourceLayout);
		}

		public GamepadBindingData GetGamepad(PlayerIndex index) {
			switch (index) {
				case PlayerIndex.One:
					return BindingManager.CurrentBindings.Gamepad1;
				case PlayerIndex.Two:
					return BindingManager.CurrentBindings.Gamepad2;
				default:
					throw new Exception("Player index is not set to 1 or 2!");
			}
		}

	}

	[Serializable]
	public struct InputSourceLayoutData {
		public bool keyboardIsPlayer2;
		public bool gamepad1IsPlayer2;

		public InputSourceLayoutData(bool kbIsP2, bool gp1IsP2) {
			keyboardIsPlayer2 = kbIsP2;
			gamepad1IsPlayer2 = gp1IsP2;
		}

		public InputSourceLayoutData(InputSourceLayoutData other) {
			keyboardIsPlayer2 = other.keyboardIsPlayer2;
			gamepad1IsPlayer2 = other.gamepad1IsPlayer2;
		}
	} 

	[Serializable]
	public class KeyboardBindingData {
		[JsonProperty] protected Dictionary<string, Keys?> Mapping;

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
		[JsonProperty] protected Dictionary<string, List<Buttons>> Mapping; // List is never null.
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

		public GamepadBindingData() {
			Mapping = new Dictionary<string, List<Buttons>>();
		}

		public GamepadBindingData(GamepadBindingData other) {
			Mapping = new Dictionary<string, List<Buttons>>(other.Mapping);
		}

		/// <summary>
		/// Bypasses checks and directly adds a new button to the mapping. Use only during initialization.
		/// </summary>
		public void Add(string key, params Buttons[] btns) {
			Mapping.Add(key, new List<Buttons>(btns));
		}

	}

}