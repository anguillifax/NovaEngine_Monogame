using Microsoft.Xna.Framework.Input;

namespace Nova.Input {

	public class VirtualKeyboardAxis : VirtualAxis {

		public VirtualKeyboardButton Pos { get; private set; }
		public VirtualKeyboardButton Neg { get; private set; }

		public string NamePos {
			get { return Name + "-pos"; }
		}
		public string NameNeg {
			get { return Name + "-neg"; }
		}

		public VirtualKeyboardAxis(string name, Keys? pos, Keys? neg) :
			base(name) {
			Pos = new VirtualKeyboardButton(NamePos, pos);
			Neg = new VirtualKeyboardButton(NameNeg, neg);
		}

		protected override void Update() {
			var v = 0f;
			v += Convert(Pos.Pressed, true);
			v += Convert(Neg.Pressed, false);
			Value = v;
		}

		/// <summary>
		/// Turn a keyboard boolean output into an axis
		/// </summary>
		private static float Convert(bool btn, bool positive) {
			return btn ? (positive ? 1f : -1f) : 0f;
		}

	}

}