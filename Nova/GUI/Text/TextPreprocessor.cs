using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui.Text {

	public static class TextPreprocessor {

		public static string PreprocessBasic(string str) {
			return str.Replace("\t", "    ");
		}

	}

}