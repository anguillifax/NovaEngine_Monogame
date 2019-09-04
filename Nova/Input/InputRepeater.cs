using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	/// <summary>
	/// Once a button is held long enough, it will retrigger in intervals.
	/// </summary>
	public class InputRepeater {

		private readonly SimpleTimer DelayUntilRepeat;
		private readonly SimpleTimer RepeatDelay;

		private readonly Func<bool> InputSource;

		private bool value, previousValue, pulse;

		public bool Pressed {
			get {
				return (value && value != previousValue) || pulse;
			}

		}

		public static implicit operator bool(InputRepeater r) => r.Pressed;

		public InputRepeater(Func<bool> inputSource) {
			InputSource = inputSource;
			DelayUntilRepeat = new SimpleTimer(InputProperties.DelayUntilRepeat);
			RepeatDelay = new SimpleTimer(InputProperties.RepeatDelay);
		}

		public void Update() {
			if (InputSource == null) {
				value = previousValue = false;
				return;
			}

			previousValue = value;
			value = InputSource();

			if (value) {

				DelayUntilRepeat.Update();
				if (DelayUntilRepeat.Running) { // Wait for intial timer to finish
					RepeatDelay.Reset();

				} else { // Pulse the repeat timer
					RepeatDelay.Update();
					pulse = false;
					if (RepeatDelay.Done) {
						RepeatDelay.Reset();
						pulse = true;
					}
				}

			} else {
				DelayUntilRepeat.Reset();
				RepeatDelay.Reset();
			}

		}

	}

}