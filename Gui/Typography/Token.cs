using System;

namespace Nova.Gui.Typography {

	public abstract class Token : IElement {

		public int Index { get; set; }

		protected Token(int index) {
			Index = index;
		}

		public abstract Token CloneToken();
		public IElement CloneElement() => CloneToken();
		public object Clone() => CloneToken();

		public void ShiftIndex(int shift) => Index += shift;

		public virtual void Consume(Typograph typograph) { }

		public override string ToString() {
			return $"Token ({BaseToString()} [{Index}])";
		}

		protected abstract string BaseToString();

	}

}