using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Input {

	public class VirtualAxis {

		public string Name { get; private set; }
		public string NamePos {
			get { return Name + "-pos"; }
		}
		public string NameNeg {
			get { return Name + "-neg"; }
		}

		public float Value { get; private set; }
		public static implicit operator float(VirtualAxis v) => v.Value;

		public bool PressedPositive { get; private set; }
		public bool PressedNegative { get; private set; }
		private float lastValue;
		public bool PressedPositiveDefaultOnly { get; private set; }
		public bool PressedNegativeDefaultOnly { get; private set; }

		private readonly SimpleTimer DelayUntilRepeat = new SimpleTimer(InputProperties.DelayUntilRepeat);
		private readonly SimpleTimer RepeatDelay = new SimpleTimer(InputProperties.RepeatDelay);

		public readonly VirtualButton keyboardPos;
		public readonly VirtualButton keyboardNeg;

		public readonly IVirtualAxisInput gamepad1;
		public readonly IVirtualAxisInput gamepad2;

		public VirtualAxis(string name, Keys kbPos, Keys kbNeg, IVirtualAxisInput gamepad1, IVirtualAxisInput gamepad2) {
			Name = name;
			keyboardPos = new VirtualButton(NamePos, kbPos) {
				IsVirtualAxisMovementKey = true
			};
			keyboardNeg = new VirtualButton(NameNeg, kbNeg) {
				IsVirtualAxisMovementKey = true
			};
			this.gamepad1 = gamepad1;
			this.gamepad2 = gamepad2;

			InputManager.InputUpdate += Update;
		}

		protected void Update() {
			lastValue = Value;

			Value = 0f;
			Value += Convert(keyboardPos.Pressed, true);
			Value += Convert(keyboardNeg.Pressed, false);
			if (gamepad1 != null) {
				Value += gamepad1.Value();
			}
			if (gamepad2 != null) {
				Value += gamepad2.Value();
			}

			float sign = Math.Sign(Value);
			Value = Math.Abs(Value);

			if (Value < InputProperties.AxisDeadzone) {
				Value = 0f;
			} else if (Value < InputProperties.AxisLowPowerThreshold) {
				Value = sign * InputProperties.AxisLowPowerAmount;
			} else {
				Value = 1f * sign;
			}

			UpdateRepeat();

		}

		private void UpdateRepeat() {
			if (Value > 0 && lastValue <= 0) {
				PressedPositive = true;
			} else if (Value < 0 && lastValue >= 0) {
				PressedNegative = true;
			} else {
				PressedPositive = PressedNegative = false;
			}

			DelayUntilRepeat.Update();
			RepeatDelay.Update();
		}

		private float Convert(bool btn, bool positive) {
			return btn ? (positive ? 1f : -1f) : 0f;
		}

	}

}