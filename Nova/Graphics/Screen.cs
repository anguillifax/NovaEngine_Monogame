using Microsoft.Xna.Framework;

namespace Nova {

	public static class Screen {

		public static int Width {
			get { return Engine.Viewport.Width; }
		}
		public static int Height {
			get { return Engine.Viewport.Height; }
		}

		public static Vector2 Size {
			get { return new Vector2(Width, Height); }
		}

		public static Vector2 Center {
			get { return Size / 2; }
		}

		public static void Update() {
		}

	}

}