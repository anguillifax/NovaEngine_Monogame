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

		public readonly InputRepeater RepeaterPos;
		public readonly InputRepeater RepeaterNeg;

		public readonly VirtualButton keyboardPos;
		public readonly VirtualButton keyboardNeg;

		public readonly IVirtualAxisInput gamepad1;
		public readonly IVirtualAxisInput gamepad2;

		public VirtualAxis(string name, Keys kbPos, Keys kbNeg, IVirtualAxisInput gamepad1, IVirtualAxisInput gamepad2) {
			Name = name;
			keyboardPos = new VirtualButton(NamePos, kbPos) {
			};
			keyboardNeg = new VirtualButton(NameNeg, kbNeg) {
			};
			this.gamepad1 = gamepad1;
			this.gamepad2 = gamepad2;

			RepeaterPos = new InputRepeater(() => Value > 0);
			RepeaterNeg = new InputRepeater(() => Value < 0);

			InputManager.InputUpdate += Update;
		}

		protected void Update() {
			// Retrieve aggregate value
			Value = 0f;
			Value += Convert(keyboardPos.Pressed, true);
			Value += Convert(keyboardNeg.Pressed, false);
			if (gamepad1 != null) {
				Value += gamepad1.Value();
			}
			if (gamepad2 != null) {
				Value += gamepad2.Value();
			}

			// Clean up output value
			float sign = Math.Sign(Value);
			Value = Math.Abs(Value);

			if (Value < InputProperties.AxisDeadzone) {
				Value = 0f;
			} else if (Value < InputProperties.AxisLowPowerThreshold) {
				Value = sign * InputProperties.AxisLowPowerAmount;
			} else {
				Value = 1f * sign;
			}

			RepeaterPos.Update();
			RepeaterNeg.Update();

		}

		/// <summary>
		/// Turn a keyboard boolean output into an axis
		/// </summary>
		private float Convert(bool btn, bool positive) {
			return btn ? (positive ? 1f : -1f) : 0f;
		}

	}

}