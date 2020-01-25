using System;

namespace Nova.Gui.Typography {

	public interface IElement : ICloneable {

		IElement CloneElement();

		void ShiftIndex(int shift);

	}

}