using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	public sealed class SpanCollection : ElementCollection<Span> {

		public static readonly SpanCollection Empty = new SpanCollection();

		public SpanCollection(IEnumerable<Span> spans) : base(spans) {
		}

		public SpanCollection(params Span[] spans) : base(spans) {
		}

		public override IOrderedEnumerable<Span> Sorted() => Elements.OrderBy(x => x.StartIndex);
		public override IOrderedEnumerable<Span> SortedCopy() => Elements.Select(x => x.CloneSpan()).OrderBy(x => x.StartIndex);

		public override string ToString() => $"Span Collection ({Elements.Count})";

	}

}