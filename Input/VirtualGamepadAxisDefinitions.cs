using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nova.Input.GamepadAxisDefinitions {

	public class StickLeftHorz : VirtualGamepadAxis.IAxisInput {
		public float Get(PlayerIndex index) {
			return GamePad.GetState(index).ThumbSticks.Left.X;
		}
	}

	public class StickLeftVert : VirtualGamepadAxis.IAxisInput {
		public float Get(PlayerIndex index) {
			return GamePad.GetState(index).ThumbSticks.Left.Y;
		}
	}

	public class StickRightHorz : VirtualGamepadAxis.IAxisInput {
		public float Get(PlayerIndex index) {
			return GamePad.GetState(index).ThumbSticks.Right.X;
		}
	}

	public class StickRightVert : VirtualGamepadAxis.IAxisInput {
		public float Get(PlayerIndex index) {
			return GamePad.GetState(index).ThumbSticks.Right.Y;
		}
	}

	public abstract class DPadToAxis : VirtualGamepadAxis.IAxisInput {

		protected abstract bool GetPos(PlayerIndex index);
		protected abstract bool GetNeg(PlayerIndex index);

		public float Get(PlayerIndex index) {
			bool pos = GetPos(index);
			bool neg = GetNeg(index);

			if (pos && neg) {
				return 0f;
			} else if (pos) {
				return 1f;
			} else if (neg) {
				return -1f;
			} else {
				return 0f;
			}
		}

	}

	public class DPadHorz : DPadToAxis {
		protected override bool GetNeg(PlayerIndex index) {
			return GamePad.GetState(index).DPad.Left == ButtonState.Pressed;

		}
		protected override bool GetPos(PlayerIndex index) {
			return GamePad.GetState(index).DPad.Right == ButtonState.Pressed;
		}
	}

	public class DPadVert : DPadToAxis {
		protected override bool GetNeg(PlayerIndex index) {
			return GamePad.GetState(index).DPad.Down == ButtonState.Pressed;

		}
		protected override bool GetPos(PlayerIndex index) {
			return GamePad.GetState(index).DPad.Up == ButtonState.Pressed;
		}
	}

}