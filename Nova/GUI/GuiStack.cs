using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	/// <summary>
	/// A GUI Stack is a simple, stack-like object designed for storing GuiElement transitions.
	/// The current element is always at the bottom of the stack.
	/// 
	/// <para>
	/// Properties
	/// <list type="bullet">
	/// <item>Only contains unique elements. No duplicate elements.</item>
	/// <item>Cannot contain null.</item>
	/// <item>Does not throw exceptions. Instead returns <c>null</c> if object does not exist.</item>
	/// <item>Illegal operations are ignored.</item>
	/// </list>
	/// </para>
	/// </summary>
	public class GuiStack {

		protected List<GuiElement> List { get; }

		/// <summary>
		/// Get the current element from bottom of stack. Returns null if element does not exit.
		/// </summary>
		public GuiElement Current => List.Count > 0 ? List[0] : null;

		/// <summary>
		/// Get the next element. Returns null if element does not exit.
		/// </summary>
		public GuiElement Next => List.Count > 1 ? List[1] : null;

		/// <summary>
		/// Total number of elements in stack.
		/// </summary>
		public int Count => List.Count;

		/// <summary>
		/// Retrieve an element from the stack using its index. Returns null if index is out of range.
		/// </summary>
		public GuiElement this[int index] => IsValidIndex(index) ? List[index] : null;


		public GuiStack() {
			List = new List<GuiElement>();
		}

		/// <summary>
		/// Attempts to push an element on to top of stack.
		/// <para>If stack already contains element or element is null, Push() does nothing</para>
		/// </summary>
		public void Push(GuiElement element) {
			if (CanPush(element)) {
				List.Add(element);
			}
		}

		/// <summary>
		/// Returns true if given element can be pushed onto stack.
		/// </summary>
		public bool CanPush(GuiElement element) {
			return element != null && !List.Contains(element);
		}

		/// <summary>
		/// Attempt to pop an element from the bottom of the stack. Returns null if stack is empty.
		/// </summary>
		public GuiElement Pop() {
			if (List.Count > 0) {
				var ret = List[0];
				List.RemoveAt(0);
				return ret;
			}
			return null;
		}

		/// <summary>
		/// Clear all elements in the stack.
		/// </summary>
		public void Clear() => List.Clear();

		/// <summary>
		/// Clear all elements above given index in stack.
		/// <para>If index is &lt; 0, clears all elements. If index > total elements, clears no elements.</para>
		/// </summary>
		public void ClearAbove(int index) {
			if (index < 0) {
				Clear();
			} else if (index < Count) {
				List.RemoveRange(index, List.Count - index);
			}
		}

		/// <summary>
		/// Clear all elements in stack except the current element.
		/// </summary>
		public void ClearNextElements() => ClearAbove(1);

		/// <summary>
		/// Returns true if stack contains element.
		/// </summary>
		public bool Contains(GuiElement element) => List.Contains(element);

		/// <summary>
		/// Returns true if index is inside of range of stack.
		/// </summary>
		private bool IsValidIndex(int index) {
			return 0 <= index && index < List.Count;
		}

	}

}