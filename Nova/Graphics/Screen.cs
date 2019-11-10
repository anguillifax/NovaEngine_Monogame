using Microsoft.Xna.Framework;

namespace Nova {

	public static class Screen {

		public static int Width => Engine.Viewport.Width;
		public static int Height => Engine.Viewport.Height;

		public static Vector2 Size => new Vector2(Width, Height);

		public static Vector2 Center => Size / 2;

		public static Rect Rect => new Rect(Vector2.Zero, Size);

		public static void Update() {
		}

	}

}