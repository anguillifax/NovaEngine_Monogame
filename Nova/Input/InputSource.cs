using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public abstract class InputSource : GameControls {

		public abstract void LoadBindings();
		public abstract void SaveBindings();

	}

	/// <summary>
	/// Packs multiple input sources into one source.
	/// </summary>
	public class CompoundInputSource : GameControls {

		public readonly List<InputSource> Sources;

		public CompoundInputSource(params InputSource[] sources) {
			Sources = new List<InputSource>(sources);

			Enter = new VirtualCompoundButton("enter");
			Back = new VirtualCompoundButton("back");
			Clear = new VirtualCompoundButton("clear");

			Jump = new VirtualCompoundButton("jump");

			UpdateVirtualButtons();
		}

		private void UpdateVirtualButtons() {
			((VirtualCompoundButton)Enter).SetNew(Sources.Select((x) => x.Enter));
			((VirtualCompoundButton)Back).SetNew(Sources.Select((x) => x.Back));
			((VirtualCompoundButton)Clear).SetNew(Sources.Select((x) => x.Clear));

			((VirtualCompoundButton)Jump).SetNew(Sources.Select((x) => x.Jump));
		}

	}

}