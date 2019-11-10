using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nova.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Nova.Gui.Text {

	public class FontCharacter {

		public char Character { get; }
		public Rectangle Rect { get; }
		public Vector2 Offset { get; }
		public int XAdvance { get; }
		public Texture2D Texture { get; }

		public FontCharacter(char character, Rectangle rect, Vector2 offset, int xAdvance, Texture2D texture) {
			Character = character;
			Rect = rect;
			Offset = offset;
			XAdvance = xAdvance;
			Texture = texture;
		}

		public void Draw(Vector2 pos, Color color, int depth) {
			MDraw.DrawGlobal(Texture, pos + Offset, Rect, color, 0, Vector2.Zero, Vector2.One, depth);
		}

		public override string ToString() {
			return $"CharDef[char: {Character}, rect: {Rect}]";
		}

	}

	public class Font {

		public string Name { get; }
		public Texture2D Texture { get; }

		public int LineHeight { get; }

		private readonly Dictionary<char, FontCharacter> Table;
		public FontCharacter this[char index] => Table[index];

		public bool Contains(char c) => Table.ContainsKey(c);
		public bool TryGet(char c, out FontCharacter fontChar) => Table.TryGetValue(c, out fontChar);

		private readonly Dictionary<Tuple<char, char>, int> KerningPairs;
		public int GetKerning(char first, char second) {
			if (KerningPairs.TryGetValue(new Tuple<char, char>(first, second), out int kerning)) {
				return kerning;
			} else {
				return 0;
			}
		}

		public Font(string path) {

			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			Name = doc.SelectSingleNode("/font/info").Attributes.GetNamedItem("face").Value;

			string textureName = doc.SelectSingleNode("/font/pages/page").Attributes.GetNamedItem("file").Value;
			Texture = Engine.Instance.Content.Load<Texture2D>("Fonts/" + Path.GetFileNameWithoutExtension(textureName));

			XmlNode commonNode = doc.SelectSingleNode("/font/common");
			LineHeight = commonNode.GetIntAttribute("lineHeight");

			Table = new Dictionary<char, FontCharacter>();
			XmlNodeList charNodes = doc.SelectNodes("/font/chars/char");
			foreach (XmlNode node in charNodes) {
				var charDef = new FontCharacter(
					(char)node.GetIntAttribute("id"),
					new Rectangle(node.GetIntAttribute("x"), node.GetIntAttribute("y"), node.GetIntAttribute("width"), node.GetIntAttribute("height")),
					new Vector2(node.GetIntAttribute("xoffset"), node.GetIntAttribute("yoffset")),
					node.GetIntAttribute("xadvance"),
					Texture);

				Table.Add(charDef.Character, charDef);
			}

			KerningPairs = new Dictionary<Tuple<char, char>, int>();
			XmlNodeList kerningNodes = doc.SelectNodes("/font/kernings/kerning");
			foreach (XmlNode node in kerningNodes) {
				KerningPairs.Add(
					new Tuple<char, char>((char)node.GetIntAttribute("first"), (char)node.GetIntAttribute("second")),
					node.GetIntAttribute("amount"));
			}

		}

		public List<FontCharacter> GetCharacters(string str, bool ignoreMissingCharacters = true) {

			var ret = new List<FontCharacter>();

			foreach (char c in str) {

				if (ignoreMissingCharacters) {

					if (Table.TryGetValue(c, out FontCharacter fontchar)) {
						ret.Add(fontchar);
					}

				} else {

					// Throw error if character not found.
					ret.Add(Table[c]);

				}

			}

			return ret;

		}

	}

}