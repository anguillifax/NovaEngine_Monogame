using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Tiles {

	//public class TestTiles : Entity {

	//	readonly Texture2D grass, dirt, deepDirt;
	//	readonly Vector2 origin;

	//	public TestTiles(Scene scene, Texture2D grass, Texture2D dirt, Texture2D deepDirt) :
	//		base(scene, Vector2.Zero) {
	//		this.grass = grass;
	//		this.dirt = dirt;
	//		this.deepDirt = deepDirt;
	//		origin = new Vector2(dirt.Width / 2, dirt.Height / 2);
	//	}

	//	public override void Draw() {
	//		MDraw.Begin();
	//		for (int x = -20; x < 20; x++) {
	//			MDraw.Draw(grass, new Vector2(x, -8), 0, origin, Vector2.One);
	//			MDraw.Draw(dirt, new Vector2(x, -9), 0, origin, Vector2.One);
	//			for (int layersDeep = 0; layersDeep < 1; layersDeep++) {
	//				MDraw.Draw(deepDirt, new Vector2(x, -10 - layersDeep), 0, origin, Vector2.One);
	//			}
	//		}
	//		MDraw.End();
	//	}

	//}

}