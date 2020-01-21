using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Typography {

	public sealed class TokenCollection : ElementCollection<Token> {

		public TokenCollection(IEnumerable<Token> tokens = null) : 
			base(tokens) {
		}

		public override IEnumerable<Token> Sorted() => Elements.OrderBy((x) => x.Index);

		public override string ToString() => $"Token Collection\n{string.Join("\n", Elements.Select(x => $"  {x}"))}";
	
 }

}