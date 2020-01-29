using Microsoft.Xna.Framework;
using Nova.Util;
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
		/// The font size to use when no font size spans are applied.
		/// </summary>
		public float DefaultFontSize { get; set; }

		/// <summary>
		/// Mapping between font name and font definition.
		/// </summary>
		public Dictionary<string, Font> FontTable { get; }

		/// <summary>
		/// Add a new font to the font table.
		/// </summary>
		public void AddFont(Font font) => FontTable.Add(font.Name, font);

		/// <summary>
		/// The root/builtin localization. Elements defined here are accessible by any localization.
		/// </summary>
		public Localization GlobalLocalization { get; }

		/// <summary>
		/// Create a new library, initializing the global localization in the process.
		/// </summary>
		public Library(string name, Color defaultTextColor, Font defaultFont, float defaultFontSize) {
			Name = name;
			DefaultTextColor = defaultTextColor;
			DefaultFont = defaultFont ?? throw new ArgumentNullException("Default font cannot be null");
			DefaultFontSize = defaultFontSize;
			FontTable = new Dictionary<string, Font>();
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

		public string ListAllLocalizations() {
			return MFormat.ToIndented(0, $"Library '{Name}' Localizations", localizations, x => $"{x.Value.Name.PadRight(12)}: parent -> '{x.Value.Parent}'");
		}

		/// <summary>
		/// Retrieve font by key. Returns default font if key not found.
		/// </summary>
		public Font GetFont(string key) {
			if (FontTable.TryGetValue(key, out Font font)) {
				return font;
			} else {
				Console.WriteLine($"[Warning] Could not find font '{key}'. Returning default font.");
				return DefaultFont;
			}
		}

		public override string ToString() => $"Typograph Library '{Name}'";

	}

}