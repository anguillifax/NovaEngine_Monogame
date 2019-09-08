using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Nova.Input {

	public static class BindingsManager {

		public static readonly string SavePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"controls.json");

		public static CombinedBindingData CurrentBindings;
		public static KeyboardBindingData DefaultKeyboardBindings;
		public static GamepadBindingData DefaultGamepadBindings;
		public static CombinedBindingData DefaultBindings;

		public static void Load() {
			try {
				CurrentBindings = SaveLoad.Load<CombinedBindingData>(SavePath);
			} catch (FileNotFoundException) {
				Console.WriteLine("No saved bindings. Loading default");
				CurrentBindings = DefaultBindings;
			}

			// TODO: load bindings

			Console.WriteLine("Loaded bindings");
		}

		public static void LoadDefault() {
			CurrentBindings = DefaultBindings;

			// TODO: load bindings

			Console.WriteLine("Loaded default bindings");
		}

		public static void Save() {
			CurrentBindings = new CombinedBindingData(
				new KeyboardBindingData(), new GamepadBindingData(PlayerIndex.One), new GamepadBindingData(PlayerIndex.Two));

			// TODO: retrieve bindings

			SaveLoad.Save(SavePath, CurrentBindings);
			Console.WriteLine("Saved bindings");
		}

		public static void CreateDefaultBindings() {

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

			//DefaultBindings[InputManager.Attack.Name] = new BindingData(Keys.F, Buttons.X);
			//DefaultBindings[InputManager.Unleash.Name] = new BindingData(Keys.D, Buttons.RightTrigger, Buttons.LeftTrigger);
			//DefaultBindings[InputManager.Restart.Name] = new BindingData(Keys.Y);
			//DefaultBindings[InputManager.Horizontal.NamePos] = new BindingData(Keys.L);
			//DefaultBindings[InputManager.Horizontal.NameNeg] = new BindingData(Keys.J);
			//DefaultBindings[InputManager.Vertical.NamePos] = new BindingData(Keys.I);
			//DefaultBindings[InputManager.Vertical.NameNeg] = new BindingData(Keys.K);
		}

	}

}