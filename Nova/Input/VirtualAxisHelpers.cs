using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Nova.Input {

	public interface IVirtualAxisInput {
		float Value();
	}

	public abstract class DPadToAxis : IVirtualAxisInput {

		protected abstract bool GetPos();
		protected abstract bool GetNeg();

		public float Value() {
			bool pos = GetPos();
			bool neg = GetNeg();

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

	public sealed class VirtualAxisInput {

		public class StickLeftHorz : IVirtualAxisInput {
			public float Value() {
				return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
			}
		}

		public class StickLeftVert : IVirtualAxisInput {
			public float Value() {
				return GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
			}
		}

		public class DPadHorz : DPadToAxis {
			protected override bool GetNeg() {
				return GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed;

			}
			protected override bool GetPos() {
				return GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed;
			}
		}

		public class DPadVert : DPadToAxis {
			protected override bool GetNeg() {
				return GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed;

			}
			protected override bool GetPos() {
				return GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed;
			}
		}

	}

}