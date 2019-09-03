using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Project.Input {

	public static class InputBindingsManager {

		public static readonly string Path = Directory.GetCurrentDirectory() + @"/Config/settings.json";

		public static Bindings CurrentBindings;
		public static readonly Bindings DefaultBindings = new Bindings();

		static InputBindingsManager() {
			CreateDefaultBindings();
		}

		public static event Action LoadBindings;
		public static event Action SaveBindings;

		public static void Test() {
			CurrentBindings = new Bindings(DefaultBindings);
			SaveLoad.Save(Path, CurrentBindings);
			CurrentBindings = SaveLoad.Load<Bindings>(Path);
			Console.WriteLine(CurrentBindings.ToString());
		}

		public static void Load() {
			LoadBindings?.Invoke();
		}

		public static void Save() {
			SaveBindings?.Invoke();
		}

		private static void CreateDefaultBindings() {
			DefaultBindings["horz-pos"] = new BindingData() {
				Keyboard = Keys.L,
			};
			DefaultBindings["horz-neg"] = new BindingData() {
				Keyboard = Keys.J,
			};
			DefaultBindings["vert-pos"] = new BindingData() {
				Keyboard = Keys.I
			};
			DefaultBindings["vert-neg"] = new BindingData() {
				Keyboard = Keys.K
			};
			DefaultBindings["jump"] = new BindingData() {
				Keyboard = Keys.Space,
				Gamepad = new List<Buttons>() { Buttons.A }
			};
			DefaultBindings["attack"] = new BindingData() {
				Keyboard = Keys.F,
				Gamepad = new List<Buttons>() { Buttons.X, Buttons.RightShoulder }
			};
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
				return Map[key];
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
			sb.AppendLine("== Bindings ==\n");
			foreach (var item in Map) {
				sb.AppendLine(item.Key);
				sb.AppendLine("   " + item.Value.Keyboard);
				sb.AppendLine("   " + PrintFormatter.ListToString(item.Value.Gamepad));
				sb.AppendLine();
			}
			return sb.ToString();
		}

	}

	[Serializable]
	public struct BindingData {
		public Keys Keyboard;
		public List<Buttons> Gamepad;
	}

}