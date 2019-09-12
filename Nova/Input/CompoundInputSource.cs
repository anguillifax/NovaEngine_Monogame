using System;
using System.Collections.Generic;

namespace Nova.Input {

	/// <summary>
	/// Packs multiple input sources into one source.
	/// </summary>
	public class CompoundInputSource : GameControls {

		public readonly List<InputSource> Sources;

		private readonly List<AutoVirtualCompoundButton> AllButtons;
		private readonly List<AutoVirtualCompoundAxis> AllAxes;

		public CompoundInputSource(params InputSource[] sources) {
			Sources = new List<InputSource>(sources);

			AllButtons = new List<AutoVirtualCompoundButton>();
			AllAxes = new List<AutoVirtualCompoundAxis>();

			CreateButton(ref Enter, new AutoVirtualCompoundButton(BindingNames.Enter, (x) => x.Enter));
			CreateButton(ref Back, new AutoVirtualCompoundButton(BindingNames.Back, (x) => x.Back));
			CreateButton(ref Clear, new AutoVirtualCompoundButton(BindingNames.Clear, (x) => x.Clear));

			CreateAxis(ref Horizontal, new AutoVirtualCompoundAxis(BindingNames.Horz, (x) => x.Horizontal));
			CreateAxis(ref Vertical, new AutoVirtualCompoundAxis(BindingNames.Vert, (x) => x.Vertical));

			CreateButton(ref Jump, new AutoVirtualCompoundButton(BindingNames.Jump, (x) => x.Jump));
			CreateButton(ref Attack, new AutoVirtualCompoundButton(BindingNames.Attack, (x) => x.Attack));
			CreateButton(ref Unleash, new AutoVirtualCompoundButton(BindingNames.Unleash, (x) => x.Unleash));
			CreateButton(ref Retry, new AutoVirtualCompoundButton(BindingNames.Retry, (x) => x.Retry));

			RetrieveNewVirtualInputs();
		}

		private void CreateButton(ref VirtualButton vb, AutoVirtualCompoundButton vcb) {
			AllButtons.Add(vcb);
			vb = vcb;
		}

		private void CreateAxis(ref VirtualAxis v, AutoVirtualCompoundAxis axis) {
			AllAxes.Add(axis);
			v = axis;
		}

		public void SetNewSources(params InputSource[] newSources) {
			Sources.ClearAdd(newSources);
			RetrieveNewVirtualInputs();
		}

		private void RetrieveNewVirtualInputs() {
			Console.WriteLine("Retrieving new virtual buttons and axes...");
			foreach (var item in AllButtons) {
				item.UpdateButtons(Sources);
			}
			foreach (var item in AllAxes) {
				item.UpdateAxes(Sources);
			}
		}

		public void SetRumble(float left, float right) {
			foreach (var source in Sources) {
				if (source is InputSourceGamepad g) {
					g.SetRumble(left, right);
				}
			}
		}

		public void SetRumble(float power) {
			SetRumble(power, power);
		}

		public void StopRumbling() {
			foreach (var source in Sources) {
				if (source is InputSourceGamepad g) {
					g.StopRumbling();
				}
			}
		}

	}

}