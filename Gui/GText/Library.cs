using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nova.Gui.GText {

	public sealed class Library {

		private readonly Dictionary<string, Locale> locales;

		private readonly GElements globalElements;

		public Library() {
			locales = new Dictionary<string, Locale>();
		}

		public Locale GetLocale(string id) {
			return locales[id];
		}

	}

}