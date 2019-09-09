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

		private readonly Func<bool> InputValue;

		private bool value, previousValue, pulse;

		public bool Pressed {
			get {
				return (value && value != previousValue) || pulse;
			}

		}

		public static implicit operator bool(InputRepeater r) => r.Pressed;

		public InputRepeater(Func<bool> inputSource) {
			InputValue = inputSource;
			DelayUntilRepeat = new SimpleTimer(GlobalInputProperties.DelayUntilRepeat);
			RepeatDelay = new SimpleTimer(GlobalInputProperties.RepeatDelay);
			DelayUntilRepeat.Reset();
			RepeatDelay.Reset();
		}

		public void Update() {
			if (InputValue == null) {
				value = previousValue = false;
				return;
			}

			pulse = false;
			previousValue = value;
			value = InputValue();

			if (value) {

				DelayUntilRepeat.Update();
				if (DelayUntilRepeat.Running) { // Wait for intial timer to finish
					RepeatDelay.Reset();

				} else { // Pulse the repeat timer
					RepeatDelay.Update();
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