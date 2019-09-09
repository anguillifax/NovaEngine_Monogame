using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public abstract class InputSource : GameControls {
	}

	/// <summary>
	/// Packs multiple input sources into one source.
	/// </summary>
	public class CompoundInputSource : GameControls {

		public List<InputSource> Sources;

		private readonly List<AutoVirtualCompoundButton> AllButtons;

		public CompoundInputSource(params InputSource[] sources) {
			Sources = new List<InputSource>(sources);

			AllButtons = new List<AutoVirtualCompoundButton>();

			CreateButton(ref Enter, new AutoVirtualCompoundButton(BindingNames.Enter, (x) => x.Enter));
			CreateButton(ref Back, new AutoVirtualCompoundButton(BindingNames.Back, (x) => x.Back));
			CreateButton(ref Clear, new AutoVirtualCompoundButton(BindingNames.Clear, (x) => x.Clear));

			CreateButton(ref Jump, new AutoVirtualCompoundButton(BindingNames.Jump, (x) => x.Jump));

			UpdateButtons();
			BindingManager.SaveBindings += UpdateButtons;
		}

		private void CreateButton(ref VirtualButton vb, AutoVirtualCompoundButton vcb) {
			AllButtons.Add(vcb);
			vb = vcb;
		}

		private void UpdateButtons() {
			if (Sources.Count > 0) {
				foreach (var item in AllButtons) {
					item.UpdateButtons(ref Sources);
				}
			}
		}

	}

}