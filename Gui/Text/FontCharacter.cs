using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

}