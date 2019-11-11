using Microsoft.Xna.Framework;
using System;

namespace Nova {

	/// <summary>
	/// Defines a virtual camera to modify drawing of scene sprites.
	/// </summary>
	public class Camera {

		/// <summary>
		/// Where the camera is located in world space.
		/// </summary>
		public Vector2 WorldPosition;

		public readonly int PixelsPerUnit;
		public readonly float UnitsPerPixel;

		public float ScaleFactor { get; set; }

		private float m_zoom;
		/// <summary>
		/// How far the camera is zoomed in. The object appears Size * Zoom units on screen.
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

		public Camera(int pixelsPerUnit) {
			m_zoom = 1;
			PixelsPerUnit = pixelsPerUnit;
			UnitsPerPixel = 1f / PixelsPerUnit;
			CalculateScale();
		}

		public void CalculateScale() {
			ScaleFactor = (float)Screen.Height / Engine.ScreenSizeInPixels.Y;
		}

		/// <summary>
		/// Transforms a position in world space into a position in global pixel space.
		/// </summary>
		public Vector2 PositionToGlobal(Vector2 pos) {
			return new Vector2(
				Screen.Width / 2 + ScaleFactor * PixelsPerUnit * Zoom * (pos.X - WorldPosition.X),
				Screen.Height / 2 - ScaleFactor * PixelsPerUnit * Zoom * (pos.Y - WorldPosition.Y));
		}

		public float ScaleTextureToGlobal(float scale) {
			return scale * Zoom * ScaleFactor;
		}

		public Vector2 ScaleTextureToGlobal(Vector2 scale) {
			return scale * Zoom * ScaleFactor;
		}

		public float ScaleSizeToGlobal(float scale) {
			return Zoom * ScaleFactor * PixelsPerUnit * scale;
		}

		public Vector2 ScaleSizeToGlobal(Vector2 scale) {
			return Zoom * ScaleFactor * PixelsPerUnit * scale;
		}

	}

}