using Microsoft.Xna.Framework;

namespace Nova {

	public class GridEntity : Entity {

		public IntVector2 GridPosition {
			get {
				return new IntVector2(Position);
			}
			set {
				Position = value.ToVector2();
			}
		}

		public GridEntity(Scene scene, IntVector2 gridPos) :
			base(scene, Vector2.Zero) {
			GridPosition = gridPos;
		}

	}

}