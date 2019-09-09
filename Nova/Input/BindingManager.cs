using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Nova.Input {

	public static class BindingManager {

		public static readonly string SavePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"controls.json");

		public static CombinedBindingData CurrentBindings;
		public readonly static KeyboardBindingData DefaultKeyboardBindings;
		public readonly static GamepadBindingData DefaultGamepadBindings;
		public readonly static CombinedBindingData DefaultBindings;

		public static event Action LoadBindings;
		public static event Action SaveBindings;

		public static void Load() {
			try {
				CurrentBindings = SaveLoad.Load<CombinedBindingData>(SavePath);
			} catch (FileNotFoundException) {
				Console.WriteLine("No saved bindings. Loading default");
				CurrentBindings = DefaultBindings;
			}

			LoadBindings?.Invoke();

			Console.WriteLine("Loaded bindings");
		}

		public static void LoadDefault() {
			CurrentBindings = DefaultBindings;

			LoadBindings?.Invoke();

			Console.WriteLine("Loaded default bindings");
		}

		public static void Save() {
			CurrentBindings = new CombinedBindingData(
				new KeyboardBindingData(), new GamepadBindingData(PlayerIndex.One), new GamepadBindingData(PlayerIndex.Two));

			SaveBindings?.Invoke();

			SaveLoad.Save(SavePath, CurrentBindings);
			Console.WriteLine("Saved bindings");
		}

		static BindingManager() {

			// Keyboard
			DefaultKeyboardBindings = new KeyboardBindingData();

			DefaultKeyboardBindings.Add("enter", Keys.F);
			DefaultKeyboardBindings.Add("jump", Keys.Space);


			// Gamepad
			DefaultGamepadBindings = new GamepadBindingData(PlayerIndex.One);

			DefaultGamepadBindings.Add("jump", Buttons.A);


			// Combination
			DefaultBindings = new CombinedBindingData(DefaultKeyboardBindings,
				DefaultGamepadBindings, new GamepadBindingData(DefaultGamepadBindings, PlayerIndex.Two));
		}

	}

}