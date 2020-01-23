﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	/// <summary>
	/// Libraries are the root element of a typography kit. Each library contains multiple localizations, which in turn contain typographs and element definitions.
	/// </summary>
	public sealed class Library {

		private readonly Dictionary<string, Localization> localizations;

		/// <summary>
		/// The name of the library.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The color assigned when no color spans are applied.
		/// </summary>
		public Color DefaultTextColor { get; set; }

		/// <summary>
		/// The font used when no font spans are applied.
		/// </summary>
		public Font DefaultFont { get; set; }

		/// <summary>
		/// The root/builtin localization. Elements defined here are accessible by any localization.
		/// </summary>
		public Localization GlobalLocalization { get; }

		/// <summary>
		/// Create a new library, initializing the global localization in the process.
		/// </summary>
		public Library(string name, Color defaultTextColor, Font defaultFont) {
			Name = name;
			DefaultTextColor = defaultTextColor;
			DefaultFont = defaultFont;
			localizations = new Dictionary<string, Localization>();
			RegisterLocalization(GlobalLocalization = Localization.CreateGlobalLocalization(this));
		}

		/// <summary>
		/// Get a localization by name.
		/// </summary>
		/// <exception cref="KeyNotFoundException" />
		public Localization GetLocalization(string name) => localizations[name];

		private void RegisterLocalization(Localization localization) => localizations.Add(localization.Name, localization);

		/// <summary>
		/// Create a new localization and add it to this library.
		/// </summary>
		public Localization CreateLocalization(string name, Localization parent = null) {
			var loc = new Localization(this, name, parent);
			RegisterLocalization(loc);
			return loc;
		}

		// DEBUG: Test printout
		public void Test() {
			foreach (var item in localizations) {
				Console.WriteLine($"{item.Key}: {item.Value} parent '{item.Value.Parent}', isglobal {item.Value.IsGlobalLocalization}");
			}
		}

		public override string ToString() => $"Typograph Library '{Name}'";

	}

}