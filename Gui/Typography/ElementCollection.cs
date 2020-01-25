using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	/// <summary>
	/// Represents a collection of spans and tokens ordered by universal indices.
	/// </summary>
	public class ElementCollection : IEnumerable<IElement>, ICloneable {

		private List<IElement> Elements { get; }

		public ElementCollection() {
			Elements = new List<IElement>();
		}

		public ElementCollection(IEnumerable<IElement> elements) {
			Elements = new List<IElement>(elements);
		}

		public ElementCollection(params IElement[] elements) : 
			this(elements.AsEnumerable()) {
		}

		/// <summary>
		/// Returns a deep copy of element collection.
		/// </summary>
		public ElementCollection(ElementCollection o) :
			this(o.GetElementsCloned()) {
		}

		public object Clone() => new ElementCollection(this);
		/// <summary>
		/// Returns a deep clone of all elements.
		/// </summary>
		public IEnumerable<IElement> GetElementsCloned() => Elements.Select(x => x.CloneElement());

		public int Count => Elements.Count;
		public int SpanCount => GetSpans().Count();
		public int TokenCount => GetTokens().Count();

		public IElement this[int index] => Elements[index];

		public void Append(IElement element) => Elements.Add(element);
		public void AppendRange(IEnumerable<IElement> elements) => Elements.AddRange(elements);

		public void Insert(int index, IElement element) => Elements.Insert(index, element);
		public void InsertRange(int index, IEnumerable<IElement> elements) => Elements.InsertRange(index, elements);

		public IEnumerable<TResult> GetByType<TResult>() => Elements.OfType<TResult>();
		public IEnumerable<Span> GetSpans() => Elements.OfType<Span>();
		public IEnumerable<Token> GetTokens() => Elements.OfType<Token>();

		public IEnumerator<IElement> GetEnumerator() {
			return Elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Elements.GetEnumerator();
		}

		public override string ToString() => $"Element Collection ({Count})";

	}

}