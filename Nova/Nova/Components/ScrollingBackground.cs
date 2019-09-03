using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Project {

	public class ScrollingBackground {

		private readonly Texture2D texture;

		/// <summary>
		/// Horizontal scroll speed in pixels per second
		/// </summary>
		public float SpeedHorizontal { get; set; }
		/// <summary>
		/// Vertical scroll speed in pixels per second
		/// </summary>
		public float SpeedVertical { get; set; }
		/// <summary>
		/// Horizontal screen position in pixels
		/// </summary>
		public float PositionHorizontal {
			get { return posHorz; }
			set {
				posHorz = value;
				if (posHorz > Screen.Width) posHorz -= Screen.Width;
				if (posHorz < 0) posHorz += Screen.Width;
			}
		}
		private float posHorz;

		/// <summary>
		/// Vertical screen position in pixels
		/// </summary>
		public float PositionVertical {
			get { return posVert; }
			set {
				posVert = value;
				if (posVert > Screen.Height) posVert -= Screen.Height;
				if (posVert < 0) posVert += Screen.Height;
			}
		}
		private float posVert;

		public ScrollingBackground(Texture2D texture) {
			this.texture = texture;
			PositionHorizontal = 0f;
			PositionVertical = 0f;
			SpeedHorizontal = 0f;
			SpeedVertical = 0f;
		}

		public void Update() {
			if (SpeedHorizontal != 0) {
				PositionHorizontal += SpeedHorizontal * Time.DeltaTime;
			}

			if (SpeedVertical != 0) {
				PositionVertical += SpeedVertical * Time.DeltaTime;
			}
		}

		public void Draw(SpriteBatch batch) {
			int xpos = (int)PositionHorizontal;
			int ypos = (int)PositionVertical;

			int splitWidth = (int)((Screen.Width - PositionHorizontal) / Screen.Width * texture.Width);
			int splitHeight = (int)((Screen.Height - PositionVertical) / Screen.Height * texture.Height);

			// Maps texture top left to screen bottom right
			Rectangle srcTopLeft = new Rectangle(0, 0, splitWidth, splitHeight);
			Rectangle destTopLeft = new Rectangle(xpos, ypos, Screen.Width - xpos, Screen.Height - ypos);
			batch.Draw(texture, destTopLeft, srcTopLeft, Color.White);

			// Maps texture top right to screen bottom left
			Rectangle srcTopRight = new Rectangle(splitWidth, 0, texture.Width - splitWidth, splitHeight);
			Rectangle destTopRight = new Rectangle(0, ypos, xpos, Screen.Height - ypos);
			batch.Draw(texture, destTopRight, srcTopRight, Color.White);

			// Maps texture bottom left to screen top right
			Rectangle srcBottomLeft = new Rectangle(0, splitHeight, splitWidth, texture.Height - splitHeight);
			Rectangle destBottomLeft = new Rectangle(xpos, 0, Screen.Width - xpos, ypos);
			batch.Draw(texture, destBottomLeft, srcBottomLeft, Color.White);

			// Maps texture bottom right to screen top left
			Rectangle srcBottomRight = new Rectangle(splitWidth, splitHeight, texture.Width - splitWidth, texture.Height - splitHeight);
			Rectangle destBottomRight = new Rectangle(0, 0, xpos, ypos);
			batch.Draw(texture, destBottomRight, srcBottomRight, Color.White);

		}

	}

}