using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	public abstract class ElementCollection<T> : IEnumerable<T> {

		protected List<T> Elements { get; }

		protected ElementCollection(IEnumerable<T> elements = null) {
			Elements = elements == null ? new List<T>() : new List<T>(elements);
		}

		public int Count => Elements.Count;


		public T this[int index] => Elements[index];

		public void Add(T element) => Elements.Add(element);

		public IEnumerable<TResult> GetByType<TResult>() => Elements.OfType<TResult>();

		/// <summary>
		/// Returns elements sorted by their indices. Order of elements with same initial indices is preserved.
		/// <para>Does not mutate the internal collection.</para>
		/// </summary>
		public abstract IEnumerable<T> Sorted();

		public IEnumerator<T> GetEnumerator() {
			return Elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Elements.GetEnumerator();
		}

	}

}