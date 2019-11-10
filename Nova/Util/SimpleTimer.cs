using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class SimpleTimer {

		public float Duration { get; set; }
		public float Current { get; private set; }

		private bool previousDone;

		/// <summary>
		/// The timer has reached 0.
		/// </summary>
		public bool Done {
			get {
				return Current <= 0;
			}
		}

		/// <summary>
		/// The timer is still running.
		/// </summary>
		public bool Running {
			get {
				return Current > 0;
			}
		}

		/// <summary>
		/// Returns true if timer reached 0 this frame.
		/// </summary>
		public bool JustFinished {
			get { return !previousDone && Done; }
		}

		/// <summary>
		/// Returns true if timer was reset this frame after completing.
		/// </summary>
		public bool JustStarted {
			get { return previousDone && !Done; }
		}

		/// <summary>
		/// Normalized progress from 0 = incomplete to 1 = complete.
		/// </summary>
		public float Progress {
			get {
				if (Duration == 0) {
					return 1f;
				} else {
					return 1 - Calc.Clamp(Current / Duration, 0, 1);
				}
			}
		}

		public SimpleTimer(float duration, bool startFinished = false) {
			Duration = duration;
			Current = startFinished ? 0 : Duration;
		}

		public SimpleTimer(SimpleTimer other) {
			Duration = other.Duration;
			Current = other.Current;
		}

		/// <summary>
		/// Prepares timer to begin counting down
		/// </summary>
		public void Reset() {
			Current = Duration;
		}

		/// <summary>
		/// Immediately finishes the countdown.
		/// </summary>
		public void End() {
			Current = 0;
		}

		public void Update(bool useDrawDeltaTime = false) {
			Update(useDrawDeltaTime ? Time.DrawDeltaTime : Time.DeltaTime);
		}

		public void Update(float delta) {
			previousDone = Done;
			if (Current >= 0) Current -= delta;
		}

	}

}