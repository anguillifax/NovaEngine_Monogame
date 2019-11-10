using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public abstract class GuiElement {

		public GuiBounds Bounds { get; }
		public bool IsExiting { get; protected set; }

		/// <summary>
		/// True if this element's exit transition cannot be interrupted.
		/// </summary>
		public bool IgnoreInterrupt { get; protected set; }

		/// <summary>
		/// Called when the Screen is resized or the containing element is resized. Use this to
		/// dynamically update the size of the element bounds.
		/// <para>This method is guaranteed to be called before OnEnter().</para>
		/// </summary>
		public abstract void Resize();

		protected GuiElement() {
			Bounds = new GuiBounds(0, 100, 0, 100);
		}

		/// <summary>
		/// Select the next element from the given elements, or return null if element should remain current element.
		/// </summary>
		public GuiElement GetNextElement(IEnumerable<GuiElement> elements) {
			foreach (var item in elements) {
				if (NextElementSelector(item)) {
					return item;
				}
			}
			return null;
		}

		/// <summary>
		/// This function is called sequentially on every element in the containing state manager.
		/// <para>Return true to set the given element as the next element.</para>
		/// </summary>
		protected abstract bool NextElementSelector(GuiElement element);

		/// <summary>
		/// Called right before the element is set as the current visible element.
		/// <para>base(): Sets IsExiting to false</para>
		/// </summary>
		public virtual void OnEnter() {
			IsExiting = false;
		}

		/// <summary>
		/// Called repeatedly when element is current element.
		/// <para>base(): empty</para>
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Called exactly once when another element is added to transition queue. Use this to trigger exit animation behavior.
		/// <para>base(): Sets IsExiting to true</para>
		/// </summary>
		public virtual void OnExit() {
			IsExiting = true;
		}

		/// <summary>
		/// Called if next element is cancelling the exit transition and inserting itself as current.
		/// <para>base(): empty</para>
		/// </summary>
		public virtual void OnInterrupt() {
		}

		/// <summary>
		/// Return true to indicate that the next element can replace the current element.
		/// </summary>
		public virtual bool IsExitTransitionFinished() {
			return true;
		}

		/// <summary>
		/// Draw the element.
		/// <para>base(): empty</para>
		/// </summary>
		public virtual void Draw() { }

	}

}