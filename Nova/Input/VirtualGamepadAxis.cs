using Microsoft.Xna.Framework;

namespace Nova.Input {

	public class VirtualGamepadAxis : VirtualAxis {

		public PlayerIndex Index { get; protected set; }

		public interface IAxisInput {
			float Get(PlayerIndex index);
		}

		private readonly IAxisInput input, input2;

		public VirtualGamepadAxis(string name, PlayerIndex index, IAxisInput input, IAxisInput input2) :
			base(name) {
			Index = index;
			this.input = input;
			this.input2 = input2;
		}

		public VirtualGamepadAxis(string name, PlayerIndex index, IAxisInput input) :
			this(name, index, input, null) {

		}

		protected override void Update() {
			if (input2 == null) {
				Value = GlobalInputProperties.CleanAxisInput(input.Get(Index));
			} else {
				Value = GlobalInputProperties.CleanAxisInput(input.Get(Index) + input2.Get(Index));
			}
		}

	}

}