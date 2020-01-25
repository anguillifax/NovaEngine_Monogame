using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.Typography {

	/// <summary>
	/// Localizations are collections of typographs and elements. All typographs are contained within a localization. Localizations inherit definitions in their parent localization.
	/// </summary>
	public sealed class Localization {

		/// <summary>
		/// The name of the global localization.
		/// </summary>
		public const string GlobalLocalizationName = "__global";

		/// <summary>
		/// Name of this localization.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The library this localization is part of.
		/// </summary>
		public Library Library { get; }

		/// <summary>
		/// The parent localization that this localization includes. Null if this is the global localization.
		/// </summary>
		public Localization Parent { get; }

		/// <summary>
		/// Returns true if this is the global localization.
		/// </summary>
		public bool IsGlobalLocalization { get; }

		// TODO: Control access?
		public Dictionary<string, Func<string>> ExternalSymbols { get; }
		public Dictionary<string, TypographData> Insertions { get; }
		public Dictionary<string, SpanCollection> Styles { get; }
		public Dictionary<string, TypographData> Typographs { get; }

		private Localization() {
			ExternalSymbols = new Dictionary<string, Func<string>>();
			Insertions = new Dictionary<string, TypographData>();
			Styles = new Dictionary<string, SpanCollection>();
			Typographs = new Dictionary<string, TypographData>();
		}

		/// <summary>
		/// Create a new localization. Parent of localization is set to the global localization if not provided.
		/// </summary>
		internal Localization(Library library, string name, Localization parent = null) :
			this() {
			Library = library;
			Parent = parent ?? Library.GlobalLocalization;
			Name = name;
			IsGlobalLocalization = false;
		}

		/// <summary>
		/// Private implementation for creating global localization.
		/// </summary>
		private Localization(Library library) :
			this() {
			Library = library;
			Parent = null;
			Name = GlobalLocalizationName;
			IsGlobalLocalization = true;
		}

		internal static Localization CreateGlobalLocalization(Library library) {
			return new Localization(library);
		}

		#region Recursive Accessors

		private void LogWarning(string warning) => Console.WriteLine("[Warning] " + warning);

		/// <summary>
		/// Create a new typograph using data associated with given key.
		/// </summary>
		/// <exception cref="KeyNotFoundException" />
		public Typograph GetTypograph(string key, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {
			if (Typographs.TryGetValue(key, out TypographData data)) {
				return new Typograph(data, topLeft, displayProperties);
			} else {
				if (Parent == null) throw new KeyNotFoundException($"Could not find specified key '{key}'");
				return Parent.GetTypograph(key, topLeft, displayProperties);
			}
		}

		/// <summary>
		/// Retrieve the value of an external symbol. Returns key as value if symbol cannot be resolved.
		/// </summary>
		public string GetExternalSymbol(string key) {
			if (ExternalSymbols.TryGetValue(key, out var symbolFunc)) {
				return symbolFunc();
			} else {
				if (Parent == null) {
					LogWarning($"Failed to resolve external symbol '{key}'. Replacing with symbol name instead.");
					return key;
				}
				return Parent.GetExternalSymbol(key);
			}
		}

		/// <summary>
		/// Retrieve a typographic insertion. Returns null if key cannot be resolved.
		/// </summary>
		public TypographData GetInsertion(string key) {
			if (Insertions.TryGetValue(key, out TypographData insertion)) {
				return insertion;
			} else {
				if (Parent == null) {
					LogWarning($"Failed to resolve insertion '{key}'.");
					return null;
				}
				return Parent.GetInsertion(key);
			}
		}

		/// <summary>
		/// Retrieve a span collection. Returns empty span if key cannot be resolved.
		/// </summary>
		public SpanCollection GetStyle(string key) {
			if (Styles.TryGetValue(key, out SpanCollection data)) {
				return data;
			} else {
				if (Parent == null) {
					LogWarning($"Failed to resolve style span '{key}'. Returning empty style collection instead.");
					return SpanCollection.Empty;
				}
				return Parent.GetStyle(key);
			}
		}

		#endregion

		public override string ToString() => $"Localization ({Name})";

	}

}