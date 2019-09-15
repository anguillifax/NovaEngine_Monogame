using Microsoft.Xna.Framework;
using System;

namespace Nova {

	/// <summary>
	/// Defines a virtual camera to modify drawing of scene sprites.
	/// </summary>
	public class Camera {

		/// <summary>
		/// Where the camera is located in global pixel coordinates.
		/// </summary>
		public Vector2 GlobalPosition;

		/// <summary>
		/// Where the camera is located in world space.
		/// </summary>
		public Vector2 WorldPosition;

		private float m_zoom;
		/// <summary>
		/// How far the camera is zoomed in. The larger, the greater the zoom.
		/// </summary>
		public float Zoom {
			get {
				return m_zoom;
			}
			set {
				m_zoom = value;
				m_zoom = Calc.Clamp(m_zoom, 0.01f, 1000f);
			}
		}

		public Camera(Vector2 globalPosition) {
			GlobalPosition = globalPosition;
			m_zoom = 1;
			WorldPosition = Vector2.Zero;
		}

		/// <summary>
		/// Transforms a position in world space into a position in global pixel space.
		/// </summary>
		public Vector2 PositionToGlobal(Vector2 pos) {
			return new Vector2(GlobalPosition.X + Zoom * (pos.X - WorldPosition.X), GlobalPosition.Y - Zoom * (pos.Y - WorldPosition.Y));
		}

		public float ScaleToGlobal(float scale) {
			return scale * Zoom;
		}

		public Vector2 ScaleToGlobal(Vector2 scale) {
			return scale * Zoom;
		}

	}

}