using Microsoft.Xna.Framework;
using Nova.Util;

namespace Nova {

	/// <summary>
	/// Represents a rectangle in space. Stored in Position-Size format in floating point.
	/// </summary>
	public class FloatRect {

		public Vector2 Position { get; set; }
		public Vector2 Size { get; set; }

		public float Top => Position.Y;
		public float Bottom => Position.Y + Size.Y;
		public float Left => Position.X;
		public float Right => Position.X + Size.X;

		public Vector2 Extents => Size / 2;
		public Vector2 Min => BottomLeft;
		public Vector2 Max => TopRight;

		public Vector2 TopLeft => new Vector2(Left, Top);
		public Vector2 TopRight => new Vector2(Right, Top);
		public Vector2 BottomLeft => new Vector2(Left, Bottom);
		public Vector2 BottomRight => new Vector2(Right, Bottom);

		public float HorzCenter => Position.X + 0.5f * Size.X;
		public float VertCenter => Position.Y + 0.5f * Size.Y;
		public Vector2 Center => new Vector2(HorzCenter, VertCenter);

		public Vector2 CenterLeft => new Vector2(Left, VertCenter);
		public Vector2 CenterRight => new Vector2(Right, VertCenter);
		public Vector2 CenterTop => new Vector2(HorzCenter, Top);
		public Vector2 CenterBottom => new Vector2(HorzCenter, Bottom);

		public Rectangle ToRectangle() {
			return new Rectangle(Position.RoundToPoint(), Size.RoundToPoint());
		}


		/// <summary>
		/// Creates a temporary rectangle at 0,0 with size 100,100
		/// </summary>
		public FloatRect() :
			this(0, 0, 100, 100) {
		}

		public FloatRect(FloatRect other) {
			Set(other);
		}

		public FloatRect(float posX, float posY, float sizeX, float sizeY) :
			this(new Vector2(posX, posY), new Vector2(sizeX, sizeY)) {
		}

		public FloatRect(Vector2 position, Vector2 size) {
			Set(position, size);
		}

		public void Set(FloatRect other) {
			Position = other.Position;
			Size = other.Size;
		}

		public void Set(Vector2 position, Vector2 size) {
			Position = position;
			Size = size;
		}

		public override string ToString() {
			return $"Rect[pos: {Position}, size: {Size}]";
		}

	}

}