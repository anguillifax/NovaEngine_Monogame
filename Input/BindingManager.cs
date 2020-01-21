using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Nova.Input {

	public static class BindingManager {

		public static readonly string SavePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"Nova Engine\controls.json");

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
				new KeyboardBindingData(), new GamepadBindingData(), new GamepadBindingData());

			SaveBindings?.Invoke();

			SaveLoad.Save(SavePath, CurrentBindings);
			Console.WriteLine("Saved bindings");
		}

		static BindingManager() {

			// Keyboard
			DefaultKeyboardBindings = new KeyboardBindingData();

			DefaultKeyboardBindings.Add(BindingNames.Enter, Keys.F);
			DefaultKeyboardBindings.Add(BindingNames.Jump, Keys.Space);
			DefaultKeyboardBindings.Add(BindingNames.Attack, Keys.F);
			DefaultKeyboardBindings.Add(BindingNames.Unleash, Keys.D);
			DefaultKeyboardBindings.Add(BindingNames.Retry, Keys.T);

			DefaultKeyboardBindings.Add(BindingNames.Horz + "-pos", Keys.L);
			DefaultKeyboardBindings.Add(BindingNames.Horz + "-neg", Keys.J);
			DefaultKeyboardBindings.Add(BindingNames.Vert + "-pos", Keys.I);
			DefaultKeyboardBindings.Add(BindingNames.Vert + "-neg", Keys.K);


			// Gamepad
			DefaultGamepadBindings = new GamepadBindingData();

			DefaultGamepadBindings.Add(BindingNames.Jump, Buttons.A);
			DefaultGamepadBindings.Add(BindingNames.Attack, Buttons.X);
			DefaultGamepadBindings.Add(BindingNames.Unleash, Buttons.LeftTrigger, Buttons.RightTrigger);
			DefaultGamepadBindings.Add(BindingNames.Retry, Buttons.RightStick);


			// Combination
			DefaultBindings = new CombinedBindingData(DefaultKeyboardBindings,
				DefaultGamepadBindings, new GamepadBindingData(DefaultGamepadBindings));

			DefaultBindings.Gamepad1.RumbleEnabled = true;
			DefaultBindings.Gamepad2.RumbleEnabled = true;
		}

	}

}