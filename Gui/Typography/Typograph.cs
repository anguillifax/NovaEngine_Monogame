using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nova.Gui.Typography {

	/// <summary>
	/// Typographs are formatted text. They are the fundamental unit of displayed text, managing line wrapping, text effects, typographic insertions, and more.
	/// Typographs are immutable, and cannot be changed once created.
	/// </summary>
	public class Typograph {

		/// <summary>
		/// Matches word boundaries, non new line whitespace, and distinct new line characters.
		/// </summary>
		private static readonly Regex RegexWord = new Regex(@"—*[^\s—]+—*|—+|[^\S\n]+|\n");
		/// <summary>
		/// Matches non new line text, and distinct new line characters.
		/// </summary>
		private static readonly Regex RegexNewLine = new Regex(@"\n|[^\n]+");

		/// <summary>
		/// The unformatted text displayed by this typograph.
		/// </summary>
		public string PlainText { get; private set; }

		private Glyph[] Glyphs { get; }

		/// <summary>
		/// The library containing this typograph. Returns null if this is an unmanaged typograph.
		/// </summary>
		public Library Library { get; }

		/// <summary>
		/// The localization containing this typograph. Returns null if this is an unmanaged typograph.
		/// </summary>
		public Localization Localization { get; }

		/// <summary>
		/// Returns true if this is a managed typograph with an associated library.
		/// </summary>
		public bool AttachedToLibrary { get; }

		private Font BaseFont => AttachedToLibrary ? Library.DefaultFont : localFont;
		private readonly Font localFont;

		private Color BaseColor => AttachedToLibrary ? Library.DefaultTextColor : localColor;
		private readonly Color localColor;

		private readonly List<IElement> elements;

		/// <summary>
		/// The top left corner of the typograph. Additive offset when combined with a display property translation matrix.
		/// </summary>
		public Vector2 TopLeft {
			get => topLeft_;
			set {
				topLeft_ = value;
				translationMatrix.Translation = new Vector3(topLeft_, 0);
			}
		}
		private Vector2 topLeft_;

		private Matrix translationMatrix;

		private TypographDisplayProperties displayProperties;

		#region Constructors

		/// <summary>
		/// Create an unmanaged typograph. Unmanaged typographs cannot contain referential elements, such as typographic insertions, external symbols, or style spans.
		/// </summary>
		public Typograph(TypographData data, Font font, Color baseColor, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {

			if (data == null) throw new ArgumentNullException("Typograph data cannot be null");
			localFont = font ?? throw new ArgumentNullException("Font cannot be null");
			localColor = baseColor;

			translationMatrix = Matrix.Identity;
			TopLeft = topLeft;

			AttachedToLibrary = false;
			Localization = null;
			Library = null;

			this.displayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);

			elements = new List<IElement>();
			ResolveReferences(data);

			Glyphs = new Glyph[PlainText.Length];

		}

		/// <summary>
		/// Create a managed typograph. Managed typographs have full access to all features of the typographic toolkit.
		/// </summary>
		public Typograph(TypographData data, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {

			if (data == null) throw new ArgumentNullException("Typograph data cannot be null");

			translationMatrix = Matrix.Identity;
			TopLeft = topLeft;

			AttachedToLibrary = true;
			Localization = data.Localization;
			Library = data.Localization.Library;

			this.displayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);

			elements = new List<IElement>();
			ResolveReferences(data);

			Glyphs = new Glyph[PlainText.Length];
		}

		#endregion

		#region Reference Resolution

		private void ResolveReferences(TypographData original) {
			StringBuilder combinedText = new StringBuilder();

			ResolveRecursive(original, 0, ref combinedText, out int _, 0);

			PlainText = combinedText.ToString();

			Console.WriteLine(TextUtil.GetRepresentation(PlainText));
			MFormat.PrintIndented(0, "Elements", elements, x => {
				if (x is Span s) return s.ToString();
				if (x is Token t) return t.ToString();
				return x.ToString();
			});
		}

		private void CWI(int depth, string text) {
			MFormat.PrintIndented(depth * 2, text);
		}

		private void ResolveRecursive(TypographData data, int textIndex, ref StringBuilder textBuilder, out int insertionLength, int depth) {

			Console.WriteLine();
			CWI(depth, "=== Begin Resolve ===");
			CWI(depth, data.ToString());
			CWI(depth, $"Plain Text: {TextUtil.GetRepresentation(data.PlainText)}");

			textBuilder.Insert(textIndex, data.PlainText);

			var curElements = new List<IElement>(data.Elements.GetElementsCloned());
			curElements.ForEach(x => x.ShiftIndex(textIndex));

			int cumulativeInsertLength = 0;

			for (int i = 0; i < curElements.Count; ++i) {

				if (curElements[i] is StyleSpan style) {

					if (AttachedToLibrary) {
						SpanCollection sc = Localization.GetStyle(style.Key);
						CWI(depth, $"Found style: {style} -> {sc}");
						sc.Spans.ForEach(x => x.StartIndex = style.StartIndex);
						sc.Spans.ForEach(x => x.Length = style.Length);
						elements.AddRange(sc);
					} else {
						CWI(depth, "[Warning] Unmanaged typograph cannot have style spans. Ignoring style span.");
					}

				} else if (curElements[i] is ExternalSymbolToken symbol) {

					string text;

					if (AttachedToLibrary) {
						text = Localization.GetExternalSymbol(symbol.Key);
					} else {
						CWI(depth, "[Warning] Unmanaged typograph cannot have external tokens. Using symbol name as value.");
						text = symbol.Key;
					}

					textBuilder.Insert(symbol.Index, text);

					for (int j = i + 1; j < curElements.Count; ++j) {
						curElements[j].ShiftIndex(text.Length);
						CWI(depth, $">> shifting {curElements[j]} by {text.Length}");
					}
					cumulativeInsertLength += text.Length;

				} else if (curElements[i] is InsertionToken insertion) {

					if (AttachedToLibrary) {
						TypographData td = Localization.GetInsertion(insertion.Key);
						if (td != null) {

							CWI(depth, $"!! Preparing to recurse '{insertion.Key}'...");
							ResolveRecursive(td, insertion.Index, ref textBuilder, out int shift, depth + 1);

							for (int j = i + 1; j < curElements.Count; ++j) {
								curElements[j].ShiftIndex(shift);
								CWI(depth, $">> shifting {curElements[j]} by {shift}");
							}

							cumulativeInsertLength += shift;

						} else {
							CWI(depth, $"[Warning] Failed to resolve insertion '{insertion.Key}'. Ignoring Insertion token.");
						}

					} else {
						CWI(depth, "[Warning] Unmanaged typograph cannot have insertion tokens. Ignoring insertion token.");
					}

				} else {
					CWI(depth, $"Appending {curElements[i]}");
					elements.Add(curElements[i]);
				}

			}

			insertionLength = data.PlainText.Length + cumulativeInsertLength;
			CWI(depth, $"Insert Length >> {insertionLength}");

			CWI(depth, "=== End Resolve ===");
			Console.WriteLine();

		}

		#endregion

		public void Update() {

		}

		public void Render() {
			MDraw.Begin(translationMatrix * displayProperties.TransformMatrix);
			foreach (var g in Glyphs) {
				g.Render();
			}
			if (displayProperties.OverflowBehavior == OverflowBehavior.Wrap) {
				MDraw.DrawRayGlobal(new Vector2(displayProperties.MaxWidth, 0), new Vector2(0, 1000), Color.DimGray);
			}
			MDraw.End();
		}

		public override string ToString() => $"Typograph ({PlainText})";

	}

}