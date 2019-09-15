using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public enum CoordinateSpace {
		Global, World, Gui
	}

	public static class MDraw {

		public static SpriteBatch SpriteBatch { get; private set; }
		public static SpriteFont DefaultFont { get; private set; }
		public static Camera Camera { get; private set; }

		private static readonly Color ShadowColor = new Color(0, 0, 0, 50);

		public static void Initialize() {
			SpriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
			Camera = new Camera(Screen.Center);
		}

		public static void LoadContent() {
			DefaultFont = Engine.Instance.Content.Load<SpriteFont>("Font1");
		}

		public static void Begin() {
			SpriteBatch.Begin(SpriteSortMode.Deferred);
		}

		public static void End() {
			SpriteBatch.End();
		}

		public static void Draw(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, Camera.PositionToGlobal(position), null, Color.White, rotation, origin, Camera.ScaleToGlobal(scale), SpriteEffects.None, 0);
		}

		public static void DrawGlobal(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, position, null, Color.White, rotation, scale, origin, SpriteEffects.None, 0);
		}

		public static void Write(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, Camera.PositionToGlobal(pos), color, 0f, Vector2.Zero, Camera.ScaleToGlobal(Vector2.One), SpriteEffects.None, 0f);
		}

		public static void WriteGlobal(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, pos, color);
		}

		#region Lines and Shapes

		public static void DrawLine(Vector2 point1, Vector2 point2, Color color) {
			DrawLineGlobal(Camera.PositionToGlobal(point1), Camera.PositionToGlobal(point2), color);
		}

		public static void DrawLineGlobal(Vector2 point1, Vector2 point2, Color color) {
			var d = point2 - point1;
			if (d.LengthSquared() < 1) return;
			Texture2D line = new Texture2D(Engine.Instance.GraphicsDevice, 1, (int)d.Length());
			//Console.WriteLine("w{0} x h{1}", line.Width, line.Height);
			var colors = new Color[line.Width * line.Height];
			for (int i = 0; i < colors.Length; i++) {
				colors[i] = color;
			}
			line.SetData(colors);
			SpriteBatch.Draw(line, point1, null, color, -(float)Math.Atan2(d.X, d.Y), Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			SpriteBatch.Draw(line, point1 + new Vector2(1, 1), null, ShadowColor, -(float)Math.Atan2(d.X, d.Y), Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
		}

		public static void DrawShapeGlobal(Color color, params Vector2[] points) {
			DrawLineGlobal(points[0], points[points.Length - 1], color);
			for (int i = 0; i < points.Length - 1; i++) {
				DrawLineGlobal(points[i], points[i + 1], color);
			}
		}

		public static void DrawShape(Color color, params Vector2[] points) {
			Array.ForEach(points, (x) => Camera.PositionToGlobal(x));
			DrawShapeGlobal(color, points);
		}

		public static void DrawBoxGlobal(Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Color color) {
			DrawShapeGlobal(color, tl, tr, br, bl);
		}

		public static void DrawBox(Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Color color) {
			DrawShape(color, tl, tr, br, bl);
		}

		public static void DrawBoxGlobal(Vector2 center, Vector2 extents, Color color) {
			DrawBoxGlobal(
				center - extents,
				new Vector2(center.X + extents.X, center.Y - extents.Y),
				new Vector2(center.X - extents.X, center.Y + extents.Y),
				center + extents,
				color);
		}

		public static void DrawBox(Vector2 center, Vector2 extents, Color color) {
			DrawBoxGlobal(Camera.PositionToGlobal(center), Camera.ScaleToGlobal(extents), color);
		}

		public static void DrawNGonGlobal(Vector2 center, float radius, int sides, Color color, float angleOffset = 0f) {
			if (sides < 3) return;
			float sector = MathHelper.TwoPi / sides;
			Vector2 lastPos = Calc.ProjectPoint(MathHelper.Pi + angleOffset, radius) + center;
			Vector2 newPos;
			for (int i = 1; i <= sides; i++) {
				newPos = Calc.ProjectPoint(i * sector + MathHelper.Pi + angleOffset, radius) + center;
				DrawLineGlobal(lastPos, newPos, color);
				lastPos = newPos;
			}
		}

		public static void DrawNGon(Vector2 center, float radius, int sides, Color color, float angleOffset = 0f) {
			DrawNGonGlobal(Camera.PositionToGlobal(center), Camera.ScaleToGlobal(radius), sides, color, angleOffset);
		}

		public static void DrawCircleGlobal(Vector2 center, float radius, Color color) {
			DrawNGonGlobal(center, radius, (int)(2 * Math.Sqrt(radius)) + 8, color);
		}

		public static void DrawCircle(Vector2 center, float radius, Color color) {
			DrawCircleGlobal(Camera.PositionToGlobal(center), Camera.ScaleToGlobal(radius), color);
		}

		#endregion

	}

}