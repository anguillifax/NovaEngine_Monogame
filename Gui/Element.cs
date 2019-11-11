using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public static class ElementUtil {

		/// <summary>
		/// Set the parent and children fields of given elements.
		/// </summary>
		public static void Adopt(Element parent, params Element[] children) {
			parent.AddChildren(children);
			foreach (var child in children) {
				child.Parent = parent;
			}
		}

	}

	public class Element {

		public Element Parent { get; set; }
		public List<Element> Children { get; }

		public void AddChildren(params Element[] children) {
			Children.AddRange(children);
		}

		public FloatRect Rect { get; }

		public Element(FloatRect rect) {
			Children = new List<Element>();
			Rect = rect;
		}

		/// <summary>
		/// Called when the window size or parent element is resized.
		/// </summary>
		public virtual void Resize() {
			foreach (var child in Children) {
				child.Resize();
			}
		}

		public virtual void Update() {
			foreach (var child in Children) {
				child.Update();
			}
		}

		public virtual void Draw() {
			MDraw.DrawRectGlobal(Rect, Color.Lime);

			foreach (var child in Children) {
				child.Draw();
			}
		}

	}

	/// <summary>
	/// Create an element that does not have a parent element.
	/// <para>Resize() automatically sets element to full size of window.</para>
	/// </summary>
	public sealed class RootElement : Element {

		public RootElement() :
			base(Screen.Rect) {
		}

		public override void Resize() {
			Rect.Set(Screen.Rect);
			base.Resize();
		}

	}

	/// <summary>
	/// An element that represents a behaviorless region of space.
	/// <para>Use GetRect function to assign rect of RegionElement without subclassing.</para>
	/// </summary>
	public sealed class RegionElement : Element {

		/// <summary>
		/// Get Rect for this element given parent element rect.
		/// </summary>
		public Func<FloatRect, FloatRect> GetRect { get; set; }

		public RegionElement(Func<FloatRect, FloatRect> getRect) :
			base(new FloatRect()) {
			GetRect = getRect;
		}

		public override void Resize() {
			if (GetRect != null) {
				Rect.Set(GetRect(Parent.Rect));
			} else {
				Rect.Set(Parent.Rect);
			}
			base.Resize();
		}
	}

}