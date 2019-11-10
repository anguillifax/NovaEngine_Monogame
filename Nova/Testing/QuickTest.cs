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
using System.Xml;

namespace Nova {

	public class QuickTest {

		IDrawableText fontString1;

		public void Init() {

		}

		public void LoadContent() {
			Font font = new Font(@"C:\Users\Bryan\Desktop\BM Font\RobotoBold.xml");
			fontString1 = new RichText(font, 700, File.ReadAllText(@"C:\Users\Bryan\Desktop\tmp.txt"));
		}

		public void Update() {
		}


		public void Draw() {
			MDraw.Begin();
			fontString1.Draw(new Vector2(30, Screen.Height / 3), Color.MonoGameOrange, MDraw.DepthStartGUI + 20);
			MDraw.End();
		}

	}

}