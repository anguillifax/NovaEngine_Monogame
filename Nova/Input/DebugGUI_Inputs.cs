using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public static class DebugGUI_Inputs {

		public static bool IsOpen { get; set; }
		static SpriteBatch b;

		public static void Update() {
			if (InputManager.InputsPanel.JustPressed) {
				IsOpen = !IsOpen;
			}
		}

		static Vector2 p;
		static Vector2 TopLeft = new Vector2(100, 20f);
		static float Column1 = 300;
		static float Column2 = 800;

		public static void Draw() {

			if (!IsOpen) return;

			b = DrawManager.SpriteBatch;
			b.Begin();

			p = TopLeft.Copy();

			Write("Inputs Panel", p, Color.White);

			p.Y += 50f;
			p.X = Column1;
			Write("Player 1", p, Color.White);

			p.X = Column2;
			Write("Player 2", p, Color.White);

			p.Y += 70f;

			WriteInput(InputManager.Player1.Enter, InputManager.Player2.Enter);
			WriteInput(InputManager.Player1.Back, InputManager.Player2.Back);
			WriteInput(InputManager.Player1.Clear, InputManager.Player2.Clear);
			WriteInput(InputManager.Player1.Jump, InputManager.Player2.Jump);

			b.End();

		}

		static void WriteInput(VirtualButton b1, VirtualButton b2) {
			p.X = TopLeft.X;
			Write(b1.Name, p, Color.White);

			p.X = Column1;
			SubWrite("P", b1.Pressed);
			SubWrite("JP", b1.JustPressed);
			SubWrite("R", b1.Released);
			SubWrite("JR", b1.JustReleased);

			p.X = Column2;
			SubWrite("P", b2.Pressed);
			SubWrite("JP", b2.JustPressed);
			SubWrite("R", b2.Released);
			SubWrite("JR", b2.JustReleased);

			p.X = TopLeft.X;
			p.Y += 50f;
		}

		static void SubWrite(string name, bool state) {
			Write(name, state ? p + new Vector2(0, 5) : p, state ? Color.Yellow : Color.White);
			p.X += 80;
		}

		private static void Write(string text, Vector2 pos, Color color) {
			b.DrawString(Engine.DefaultFont, text, pos, color);
		}

	}

}