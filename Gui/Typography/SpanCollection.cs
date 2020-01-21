using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	public sealed class SpanCollection : ElementCollection<Span> {

		public SpanCollection(IEnumerable<Span> spans = null) :
			base(spans) {
		}

		public override IEnumerable<Span> Sorted() => Elements.OrderBy((x) => x.StartIndex);

		public override string ToString() => $"Span Collection\n{string.Join("\n", Elements.Select(x => $"  {x}"))}";

	}

}