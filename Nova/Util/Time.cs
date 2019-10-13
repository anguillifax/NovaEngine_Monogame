using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Nova {

	public static class Time {

		public static float DeltaTime { get; private set; }
		public static float TotalTime { get; private set; }

		public static float DrawDeltaTime { get; private set; }

		public static float ExactTimeSinceStartup => GetDifference(DateTime.Now, ExactTimeOfStartup);
		public static DateTime ExactTimeOfStartup { get; private set; }
		public static float ExactTimeOfUpdate { get; private set; }
		public static float ExactTimeOfDraw { get; private set; }

		internal static void Init() {
			ExactTimeOfStartup = DateTime.Now;
		}

		internal static void Update(GameTime time) {
			ExactTimeOfUpdate = ExactTimeSinceStartup;

			DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
			TotalTime = (float)time.TotalGameTime.TotalSeconds;
		}

		internal static void UpdateDraw(GameTime time) {
			ExactTimeOfDraw = ExactTimeSinceStartup;

			DrawDeltaTime = (float)time.ElapsedGameTime.TotalSeconds;
		}

		public static float GetDifference(DateTime current, DateTime last) {
			return (float)current.Subtract(last).TotalSeconds;
		}

	}

}