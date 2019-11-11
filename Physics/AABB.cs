using Microsoft.Xna.Framework;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Represents an Axis Aligned Bounding Box.
	/// </summary>
	public struct AABB {

		public Vector2 Position { get; set; }
		public Vector2 Extents { get; set; }

		public Vector2 Size {
			get => 2 * Extents;
			set => Extents = value / 2;
		}

		public Vector2 Max => Position + Extents;
		public Vector2 Min => Position - Extents;

		public AABB(Vector2 center, Vector2 extents) {
			Position = center;
			Extents = extents;
		}

		public AABB(AABB other) {
			Position = other.Position;
			Extents = other.Extents;
		}

		/// <summary>
		/// Returns true if point is inside of AABB.
		/// </summary>
		public bool IsWithin(Vector2 point) {
			if (point.X < Min.X || point.X > Max.X) return false;
			if (point.Y < Min.Y || point.Y > Max.Y) return false;
			return true;
		}

		public void Draw(Color color) {
			Draw(Vector2.Zero, color);
		}

		public void Draw(Vector2 offset, Color color) {
			MDraw.DrawBox(offset + Position, Extents, color);
			MDraw.WriteTiny(Position.ToString(), offset + Position, Color.Gray);
		}

	}

}