using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public static class MDraw {

		public static SpriteBatch SpriteBatch { get; private set; }
		public static SpriteFont DefaultFont { get; private set; }
		public static Camera Camera { get; private set; }

		private static readonly Color ShadowColor = new Color(0, 0, 0, 30);

		private static Texture2D TexturePixel;
		private static Texture2D TexturePoint;

		public const int DepthStartEngineReserved = 0;
		public const int DepthStartGUI = 100;
		public const int DepthStartScene = 200;

		public static void Initialize() {
			SpriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
			
			Camera = new Camera(Engine.TileSize);
		}

		public static void LoadContent() {
			DefaultFont = Engine.Instance.Content.Load<SpriteFont>("Font1");

			TexturePixel = new Texture2D(Engine.Instance.GraphicsDevice, 1, 1);
			TexturePixel.SetData(new Color[] { Color.White });

			TexturePoint = new Texture2D(Engine.Instance.GraphicsDevice, 3, 3);
			var colors = new Color[3 * 3];
			for (int i = 0; i < colors.Length; i++) {
				colors[i] = Color.White;
			}
			TexturePoint.SetData(colors);
		}

		public static void Begin() {
			SpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState:SamplerState.PointClamp);
		}

		public static void End() {
			SpriteBatch.End();
		}

		private static float GetDepth(int depth) {
			return depth / 1000f;
		}

		#region Draw Methods

		public static void Draw(Texture2D texture, int depth, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, Camera.PositionToGlobal(position), null, Color.White, rotation, origin, Camera.ScaleTextureToGlobal(scale), SpriteEffects.None, GetDepth(depth));
		}

		public static void DrawGlobal(Texture2D texture, Vector2 position, float rotation, Vector2 origin, Vector2 scale) {
			SpriteBatch.Draw(texture, position, null, Color.White, rotation, scale, origin, SpriteEffects.None, 0);
		}

		public static void Write(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, Camera.PositionToGlobal(pos), color, 0f, Vector2.Zero, Camera.ScaleTextureToGlobal(Vector2.One), SpriteEffects.None, 0f);
		}

		public static void WriteGlobal(string text, Vector2 pos, Color color) {
			SpriteBatch.DrawString(DefaultFont, text, pos, color);
		}

		#endregion

		#region Lines and Shapes

		public static void DrawLine(Vector2 point1, Vector2 point2, Color color) {
			DrawLineGlobal(Camera.PositionToGlobal(point1), Camera.PositionToGlobal(point2), color);
		}

		public static void DrawLineGlobal(Vector2 origin, Vector2 point, Color color) {
			var d = point - origin;
			if (d.LengthSquared() < 1) return;
			float angle = -MathHelper.PiOver2 + (float)Math.Atan2(d.Y, d.X);
			Vector2 scale = new Vector2(1, (int)d.Length());
			SpriteBatch.Draw(TexturePixel, origin, null, color, angle, new Vector2(0.5f, 0), scale, SpriteEffects.None, GetDepth(2));
			SpriteBatch.Draw(TexturePixel, origin + Vector2.One, null, ShadowColor, angle, new Vector2(0.5f, 0), scale, SpriteEffects.None, GetDepth(3));
		}

		public static void DrawPointGlobal(Vector2 pos, Color color) {
			SpriteBatch.Draw(TexturePoint, pos, null, color, 0, new Vector2(1.5f, 1.5f), Vector2.One, SpriteEffects.None, GetDepth(0));
			SpriteBatch.Draw(TexturePoint, pos + Vector2.One, null, ShadowColor, 0, new Vector2(1.5f, 1.5f), Vector2.One, SpriteEffects.None, GetDepth(1));
		}

		public static void DrawPoint(Vector2 pos, Color color) {
			DrawPointGlobal(Camera.PositionToGlobal(pos), color);
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
			DrawBoxGlobal(Camera.PositionToGlobal(center), Camera.ScaleSizeToGlobal(extents), color);
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
			DrawNGonGlobal(Camera.PositionToGlobal(center), Camera.ScaleSizeToGlobal(radius), sides, color, angleOffset);
		}

		public static void DrawCircleGlobal(Vector2 center, float radius, Color color) {
			DrawNGonGlobal(center, radius, (int)(2 * Math.Sqrt(radius)) + 8, color);
		}

		public static void DrawCircle(Vector2 center, float radius, Color color) {
			DrawCircleGlobal(Camera.PositionToGlobal(center), Camera.ScaleSizeToGlobal(radius), color);
		}

		#endregion

	}

}