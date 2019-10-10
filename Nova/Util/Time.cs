﻿using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Nova {

	public static class Time {

		public static float DeltaTime { get; private set; }
		public static float TotalTime { get; private set; }

		public static float DrawDeltaTime { get; private set; }


		internal static void Update(GameTime time) {
			DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
			TotalTime = (float)time.TotalGameTime.TotalSeconds;
		}

		internal static void UpdateDraw(GameTime time) {
			DrawDeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
		}

	}

}