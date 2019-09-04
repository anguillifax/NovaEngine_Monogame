using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Nova.Input {

	public static class InputBindingsManager {

		public static readonly string Path = Directory.GetCurrentDirectory() + @"/Config/settings.json";

		public static Bindings CurrentBindings;
		public static readonly Bindings DefaultBindings = new Bindings();

		public static event Action LoadBindings;
		public static event Action SaveBindings;

		public static void Load() {
			try {
				CurrentBindings = SaveLoad.Load<Bindings>(Path);
			} catch (FileNotFoundException) {
				Console.WriteLine("No saved bindings. Loading default");
				CurrentBindings = DefaultBindings;
			}
			LoadBindings?.Invoke();
			Console.WriteLine(CurrentBindings);
			Console.WriteLine("Loaded bindings");
		}

		public static void LoadDefault() {
			CurrentBindings = DefaultBindings;
			LoadBindings?.Invoke();
			Console.WriteLine(CurrentBindings);
			Console.WriteLine("Loaded default bindings");
		}

		public static void Save() {
			CurrentBindings = new Bindings();
			SaveBindings?.Invoke();
			SaveLoad.Save(Path, CurrentBindings);
			Console.WriteLine(CurrentBindings);
			Console.WriteLine("Saved bindings");
		}

		public static void CreateDefaultBindings() {
			DefaultBindings[InputManager.Enter.Name] = new BindingData(Keys.F);
			DefaultBindings[InputManager.Jump.Name] = new BindingData(Keys.Space, Buttons.A);
			DefaultBindings[InputManager.Attack.Name] = new BindingData(Keys.F, Buttons.X);
			DefaultBindings[InputManager.Unleash.Name] = new BindingData(Keys.D, Buttons.RightTrigger, Buttons.LeftTrigger);
		}

	}

}