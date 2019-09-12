using System;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	/// <summary>
	/// Packs multiple axis outputs into a single axis.
	/// </summary>
	public class VirtualCompoundAxis : VirtualAxis {

		public readonly List<VirtualAxis> SubAxes;

		public VirtualCompoundAxis(string name, params VirtualAxis[] axes) :
			base(name) {
			SubAxes = new List<VirtualAxis>(axes);
		}

		protected override void Update() {
			var v = 0f;
			foreach (var item in SubAxes) {
				v += item.Value;
			}
			Value = GlobalInputProperties.CleanAxisInput(v);
		}

		/// <summary>
		/// Clears all previous axes and sets new axes.
		/// </summary>
		public void SetNew(IEnumerable<VirtualAxis> axes) {
			SubAxes.ClearAdd(axes);
		}

	}

	/// <summary>
	/// Uses a Linq query to automatically retrieve new axes.
	/// </summary>
	public class AutoVirtualCompoundAxis : VirtualCompoundAxis {

		/// <summary>
		/// Defines what VirtualButton to retrieve from an InputSource.
		/// </summary>
		private readonly Func<InputSource, VirtualAxis> selector;

		public AutoVirtualCompoundAxis(string name, Func<InputSource, VirtualAxis> func, params VirtualAxis[] axes) :
			base(name, axes) {
			selector = func;
		}

		public void UpdateAxes(List<InputSource> sources) {
			SetNew(sources.Select(selector));
		}

	}

}