using Microsoft.Xna.Framework;

namespace Nova {

	public abstract class VisualEntity : Entity {

		protected VisualEntity(Scene scene, bool startVisible = true) :
			base(scene) {
			Visible = startVisible;
			Position = Vector2.Zero;
			Scale = Vector2.One;
			Rotation = 0f;
		}

		public bool Visible { get; set; }

		private Vector2 m_Position;
		public Vector2 Position { get => m_Position; set => m_Position = value; }
		private Vector2 m_Scale;
		public Vector2 Scale { get => m_Scale; set => m_Scale = value; }
		public float Rotation { get; set; }

		#region Transform Component Properties

		public float PosX {
			get => Position.X;
			set => m_Position.X = value;
		}
		public float PosY {
			get => Position.Y;
			set => m_Position.Y = value;
		}

		public float ScaleX {
			get => Scale.X;
			set => m_Scale.X = value;
		}
		public float ScaleY {
			get => Scale.Y;
			set => m_Scale.Y = value;
		}

		#endregion

		public virtual void Draw() {
		}

	}

}