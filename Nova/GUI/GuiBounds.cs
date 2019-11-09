using Microsoft.Xna.Framework;

namespace Nova.Gui {

	public class GuiBounds {

		public Vector2 TopLeft => new Vector2(Left, Top);
		public Vector2 TopRight => new Vector2(Right, Top);
		public Vector2 BottomLeft => new Vector2(Left, Bottom);
		public Vector2 BottomRight => new Vector2(Right, Bottom);

		public Vector2 Center => new Vector2(HorzCenter, VertCenter);

		public Vector2 CenterLeft => new Vector2(Left, VertCenter);
		public Vector2 CenterRight => new Vector2(Right, VertCenter);
		public Vector2 CenterTop => new Vector2(HorzCenter, Top);
		public Vector2 CenterBottom => new Vector2(HorzCenter, Bottom);

		public float Top { get; set; }
		public float Bottom { get; set; }
		public float Left { get; set; }
		public float Right { get; set; }

		public float HorzCenter => 0.5f * (Right - Left);
		public float VertCenter => 0.5f * (Top - Bottom);

		public GuiBounds(float top, float bottom, float left, float right) {
			SetBounds(top, bottom, left, right);
		}

		public void SetBounds(float top, float bottom, float left, float right) {
			Top = top;
			Bottom = bottom;
			Left = left;
			Right = right;
		}

		public override string ToString() {
			return $"Bounds[top: {Top}, bottom: {Bottom}, left: {Left}, right: {Right}]";
		}

	}

}