using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class TestEntity : Image {

		public TestEntity(Texture2D tex) : base(tex) {
		}

		public override void Update() {
			Position += new Vector2(0, 10 * Time.DeltaTime);
			Console.WriteLine("Updated test entity " + Position);
			base.Update();
		}

	}

}