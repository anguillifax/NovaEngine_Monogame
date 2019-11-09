using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public abstract class GuiElement {

		public GuiBounds Bounds { get; }

		public GuiElement() {
			Bounds = new GuiBounds(0, 100, 0, 100);
		}

		public abstract void Resize();

		public abstract Type GetNext();

		public virtual void OnEnter() { }
		public virtual void Update() { }
		public virtual void OnExit() { }

		public virtual bool IsExitTransitionFinished() {
			return true;
		}

		public virtual void Draw() { }

	}

}