using Microsoft.Xna.Framework;

namespace Project {

	public static class Screen {

		public static int Width { get; private set; }
		public static int Height { get; private set; }

		public static Vector2 Size {
			get { return new Vector2(Width, Height); }
		}

		public static Vector2 Center {
			get { return 0.5f * Size; }
		}

		public static void Update(Rectangle viewport) {
			Width = viewport.Width;
			Height = viewport.Height;
		}

	}

}