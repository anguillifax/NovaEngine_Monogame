using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	public sealed class SpanCollection : IEnumerable<Span> {

		public static readonly SpanCollection Empty = new SpanCollection();

		public List<Span> Spans { get; }

		public SpanCollection() {
			Spans = new List<Span>();
		}

		public SpanCollection(IEnumerable<Span> spans) {
			Spans = new List<Span>(spans);
		}

		public SpanCollection(params Span[] spans) : 
			this(spans.AsEnumerable()) {
		}

		public IOrderedEnumerable<Span> Sorted() => Spans.OrderBy(x => x.StartIndex);
		public IOrderedEnumerable<Span> SortedCopy() => Spans.Select(x => x.CloneSpan()).OrderBy(x => x.StartIndex);

		public override string ToString() => $"Span Collection ({Spans.Count})";

		public IEnumerator<Span> GetEnumerator() {
			return Spans.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Spans.GetEnumerator();
		}
	}

}