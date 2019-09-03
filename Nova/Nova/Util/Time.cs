using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Project {

	public static class Time {

		public static float DeltaTime { get; private set; }
		public static double TotalTime { get; private set; }

		internal static void Update(GameTime time) {
			DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
			TotalTime = time.TotalGameTime.TotalSeconds;
		}

	}

}