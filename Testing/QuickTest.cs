using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nova.Gui.Text;
using Nova.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Nova {

	public class QuickTest {

		IDrawableText fontText;
		Font font;
		string text;

		float width = 300;
		int height = 0;

		public void Init() {
		}

		public void LoadContent() {
			font = new Font(@"C:\Users\Bryan\Desktop\BM Font\RobotoBold.xml");
			text = File.ReadAllText(@"C:\Users\Bryan\Desktop\tmp.txt");
			CreateText();
		}

		public void Update() {

			width += 300 * Time.DeltaTime * InputManager.Any.Horizontal.Value;

			if (InputManager.Any.Horizontal.Value != 0) {
				CreateText();
			}

		}

		void CreateText() {
			var lines = TextSplitter.SplitText(font, TextPreprocessor.PreprocessBasic(text), (int)width);
			fontText = new FontDraw(font, lines);
			height = font.LineHeight * lines.Count;
		}


		public void Draw() {

			if (fontText != null) {

				MDraw.Begin();
				Vector2 pos = new Vector2(30, 50);

				MDraw.DrawBoxGlobalMinMax(pos, pos + new Vector2((int)width, height), Color.Gray);
				fontText.Draw(pos, Color.MonoGameOrange, MDraw.DepthStartGUI + 20);
				MDraw.End();

			}

		}

	}

}