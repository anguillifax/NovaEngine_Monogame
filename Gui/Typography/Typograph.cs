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

		private readonly Font localFont;
		private readonly Color localColor;
		private readonly float localFontSize;

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
		public Typograph(TypographData data, Font font, Color defaultColor, float defaultFontSize, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {

			if (data == null) throw new ArgumentNullException("Typograph data cannot be null");
			localFont = font ?? throw new ArgumentNullException("Font cannot be null");
			localColor = defaultColor;
			localFontSize = defaultFontSize;

			translationMatrix = Matrix.Identity;
			TopLeft = topLeft;

			AttachedToLibrary = false;
			Localization = null;
			Library = null;

			this.displayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);

			elements = new List<IElement>();
			ResolveReferences(data);

			Glyphs = new Glyph[PlainText.Length];
			CreateGlyphs();
			PositionGlyphs();
			InitializeElements();
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
			CreateGlyphs();
			PositionGlyphs();
			InitializeElements();

			MFormat.PrintIndented("Elements", elements);
		}

		#endregion

		#region Reference Resolution

		private void ResolveReferences(TypographData original) {
			StringBuilder combinedText = new StringBuilder();

			ResolveRecursive(original, 0, ref combinedText, out int _, 0);

			PlainText = combinedText.ToString();

			Console.WriteLine("=== Start Reference Resolution Printout ===");
			Console.WriteLine(TextUtil.GetRepresentation(PlainText));
			MFormat.PrintIndented(0, "Elements", elements, x => {
				if (x is Span s) return s.ToString();
				if (x is Token t) return t.ToString();
				return x.ToString();
			});
			Console.WriteLine("=== End Reference Resolution Printout ===\n");
		}

		internal static bool traceResolve = true;

		private void CWI(int depth, string text) {
			MFormat.PrintIndented(depth * 2, text);
		}

		private void CorrectIndicesCurrent(ref List<IElement> curElements, int uindex, int textIndex, int length, int depth) {
			for (int i = 0; i < uindex + 1; ++i) {
				if (curElements[i] is Span s && (s.StopIndex > textIndex || (s.StartIndex == textIndex && s.Length == 0))) {
					s.Length += length;
					if (traceResolve) CWI(depth, $">> extending {s} by {length}");
				}
			}
			for (int i = uindex + 1; i < curElements.Count; ++i) {
				curElements[i].ShiftIndex(length);
				if (traceResolve) CWI(depth, $">> shifting {curElements[i]} by {length}");
			}
		}

		private void ResolveRecursive(TypographData data, int textIndex, ref StringBuilder textBuilder, out int insertionLength, int depth) {

			if (traceResolve) {
				Console.WriteLine();
				CWI(depth, "=== Begin Resolve ===");
				CWI(depth, data.ToString());
				CWI(depth, $"Plain Text: {TextUtil.GetRepresentation(data.PlainText)}");
			}

			textBuilder.Insert(textIndex, data.PlainText);

			var curElements = new List<IElement>(data.Elements.GetElementsCloned());
			curElements.ForEach(x => x.ShiftIndex(textIndex));

			int cumulativeInsertLength = 0;

			for (int i = 0; i < curElements.Count; ++i) {

				if (curElements[i] is StyleSpan style) {

					if (AttachedToLibrary) {
						SpanCollection sc = Localization.GetStyle(style.Key);
						if (traceResolve) CWI(depth, $"Found style: {style} -> {sc}");
						sc.Spans.ForEach(x => x.StartIndex = style.StartIndex);
						sc.Spans.ForEach(x => x.Length = style.Length);
						elements.AddRange(sc);
					} else {
						CWI(depth, $"[Warning] Unmanaged typograph cannot have style spans. Ignoring style span '{style}'.");
					}

				} else if (curElements[i] is ExternalSymbolToken symbol) {

					string text;

					if (AttachedToLibrary) {
						text = Localization.GetExternalSymbol(symbol.Key);
					} else {
						CWI(depth, $"[Warning] Unmanaged typograph cannot have external tokens. Replacing '{symbol}' with name as value.");
						text = symbol.Key;
					}

					textBuilder.Insert(symbol.Index, text);

					CorrectIndicesCurrent(ref curElements, i, symbol.Index, text.Length, depth);

					cumulativeInsertLength += text.Length;

				} else if (curElements[i] is InsertionToken insertion) {

					if (AttachedToLibrary) {
						TypographData td = Localization.GetInsertion(insertion.Key);
						if (td != null) {

							if (traceResolve) CWI(depth, $"!! Preparing to recurse '{insertion.Key}'...");
							ResolveRecursive(td, insertion.Index, ref textBuilder, out int shift, depth + 1);

							CorrectIndicesCurrent(ref curElements, i, insertion.Index, shift, depth);

							cumulativeInsertLength += shift;

						} else {
							CWI(depth, $"[Warning] Failed to resolve insertion '{insertion.Key}'. Ignoring Insertion token.");
						}

					} else {
						CWI(depth, $"[Warning] Unmanaged typograph cannot have insertion tokens. Ignoring insertion token '{insertion}'.");
					}

				} else {
					if (ShouldAdd(curElements[i])) {
						if (traceResolve) CWI(depth, $"Appending {curElements[i]}");
						elements.Add(curElements[i]);
					}
				}

			}

			insertionLength = data.PlainText.Length + cumulativeInsertLength;
			if (traceResolve) {
				CWI(depth, $"Insert Length >> {insertionLength}");

				CWI(depth, "=== End Resolve ===");
				Console.WriteLine();
			}
		}

		private static readonly HashSet<Type> UnmanagedElementBlacklist = new HashSet<Type>() { typeof(FontSpan) };

		/// <summary>
		/// Returns false on referential elements in unmanaged typographs.
		/// </summary>
		private bool ShouldAdd(IElement element) {
			return !(!AttachedToLibrary && UnmanagedElementBlacklist.Contains(element.GetType()));
		}

		#endregion

		#region Glyph Creation and Alignment

		private void CreateGlyphs() {

			List<FontSizeSpan> fontSizeSpans = new List<FontSizeSpan>(elements.OfType<FontSizeSpan>());

			if (AttachedToLibrary) {

				List<FontSpan> fontSpans = new List<FontSpan>(elements.OfType<FontSpan>());

				for (int i = 0; i < PlainText.Length; ++i) {
					int fontIdx = fontSpans.FindLastIndex(x => x.IsInside(i));
					Glyphs[i] = fontIdx != -1 ? Library.GetFont(fontSpans[fontIdx].FontKey).GetGlyph(PlainText[i]) : Library.DefaultFont.GetGlyph(PlainText[i]);
					int sizeIdx = fontSizeSpans.FindLastIndex(x => x.IsInside(i));
					Glyphs[i].Size = sizeIdx != -1 ? fontSizeSpans[sizeIdx].Size : Library.DefaultFontSize;
				}

			} else {

				if (elements.Any(x => x is FontSpan)) {
					Console.WriteLine("[Warning] Unmanaged typographs cannot use font spans.");
				}
				for (int i = 0; i < PlainText.Length; ++i) {
					Glyphs[i] = localFont.GetGlyph(PlainText[i]);
					int idx = fontSizeSpans.FindLastIndex(x => x.IsInside(i));
					Glyphs[i].Size = idx != -1 ? fontSizeSpans[idx].Size : localFontSize;
				}

			}

			//MFormat.PrintIndented("Fonts", Glyphs, x => $"{x.Character}\t{x.Font}");

		}

		/// <summary>
		/// Recalculate layout of characters based on new display options.
		/// </summary>
		public void Redraw(TypographDisplayProperties properties) {
			displayProperties = properties;
			PositionGlyphs();
		}

		private void PositionGlyphs() {
			GlyphPosUtil.Run(this);
		}

		/// <summary>
		/// Small internal class that handles details of glyph positioning.
		/// </summary>
		private class GlyphPosUtil {

			/// <summary>
			/// Matches word boundaries, non new line whitespace, and distinct new line characters.
			/// </summary>
			private static readonly Regex RegexWord = new Regex(@"—*[^\s—]+—*|—+|[^\S\n]+|\n");
			/// <summary>
			/// Matches non new line text, and distinct new line characters.
			/// </summary>
			private static readonly Regex RegexNewLine = new Regex(@"\n|[^\n]+");

			private readonly string plaintext;
			private readonly Typograph typograph;
			private readonly List<List<Glyph>> glyphLines;
			private readonly Glyph[] glyphs;


			public static void Run(Typograph typograph) => new GlyphPosUtil(typograph).DoPositioning();

			private GlyphPosUtil(Typograph typograph) {
				this.typograph = typograph;
				plaintext = typograph.PlainText;
				glyphLines = new List<List<Glyph>>();
				glyphs = typograph.Glyphs;
			}

			private void DoPositioning() {

				// Position glyphs and create line divisions.
				switch (typograph.displayProperties.OverflowBehavior) {
					case OverflowBehavior.Extend:
						ExtendMode();
						break;
					case OverflowBehavior.Wrap:
						WrapMode();
						break;
					default:
						ExtendMode();
						break;
				}

				ShiftNewLines();

				//Console.WriteLine("Glyph Lines");
				//for (int i = 0; i < glyphLines.Count; ++i) {
				//	MFormat.PrintIndented(1, $"Line {i}", glyphLines[i]);
				//}
			}

			private void ExtendMode() {

				float curXPos = 0;
				int lineStart = 0;
				int lineLength = 0;

				for (int i = 0; i < glyphs.Length; ++i) {

					++lineLength;

					Glyph cur = glyphs[i];

					cur.Position = new Vector2(curXPos, 0);
					curXPos += cur.XAdvance;

					if (i < glyphs.Length - 1) {
						Glyph next = glyphs[i + 1];
						curXPos += next.Font.GetKerning(cur.Character, next.Character) * Math.Min(cur.Factor, next.Factor);
					}

					if (cur.Character == '\n' || i == glyphs.Length - 1) {
						glyphLines.Add(new List<Glyph>(glyphs.Skip(lineStart).Take(lineLength)));
						lineStart = i + 1;
						lineLength = 0;
						curXPos = 0;

					}

				}

			}

			private void WrapMode() {

				MatchCollection matches = RegexWord.Matches(plaintext);

				List<NonBreakingSequenceSpan> uniquenbs = GetUniqueNBS();
				MFormat.PrintIndented("Unique NBS", uniquenbs);

				List<NonBreakingSequenceSpan> sequences = new List<NonBreakingSequenceSpan>();

				int nbsIndex = 0;
				for (int i = 0; i < matches.Count; ++i) {

					Match m = matches[i];

				}



			}

			private List<NonBreakingSequenceSpan> GetUniqueNBS() {

				var allnbs = new List<NonBreakingSequenceSpan>(typograph.elements.OfType<NonBreakingSequenceSpan>());
				List<NonBreakingSequenceSpan> unique = new List<NonBreakingSequenceSpan>();

				for (int i = 0; i < allnbs.Count; ++i) {

					NonBreakingSequenceSpan cur = allnbs[i];
					int length = cur.Length;

					for (int j = 1; j < allnbs.Count - i; ++j) {
						if (allnbs[i + j].StartIndex < cur.StartIndex + length) {
							length = Math.Max(length, allnbs[i + j].StopIndex - cur.StartIndex);
							++i;
						}
					}

					unique.Add(new NonBreakingSequenceSpan(cur.StartIndex, length));

				}

				return unique;

			}

			/// <summary>
			/// Finds the tallest glyph in the current line and adjusts the line's y position.
			/// </summary>
			private void ShiftNewLines() {

				float yPos = 0;

				for (int i = 0; i < glyphLines.Count; ++i) {
					if (i > 0) {
						float m = glyphLines[i - 1].Take(glyphLines.Count - 1).Max(x => x.LineHeight);
						Console.WriteLine($"yadvance to {string.Concat(glyphLines[i].Select(x => TextUtil.GetRepresentation(x.Character)))} by {m}");
						yPos += m;
					}
					glyphLines[i].ForEach(x => x.Position += new Vector2(0, yPos));
				}

			}

		}

		#endregion

		private void InitializeElements() {
			Array.ForEach(Glyphs, x => x.Color = AttachedToLibrary ? Library.DefaultTextColor : localColor);

			var colorSpans = new List<ColorSpan>(elements.OfType<ColorSpan>());

			for (int i = 0; i < Glyphs.Length; ++i) {

				int idx = colorSpans.FindLastIndex(x => x.IsInside(i));
				if (idx != -1) Glyphs[i].Color = colorSpans[idx].Color;

			}

			//MFormat.PrintIndented("Colors", Glyphs, x => $"{x.Character}\t{x.Color.ToHex()}\t{x.Position}");
		}

		public void Update() {

		}

		private static readonly Color DebugGraphicsColor = ColorUtil.FromHex("666");

		public void Render() {
			MDraw.Begin();
			MDraw.DrawPointGlobal(TopLeft, DebugGraphicsColor);
			MDraw.WriteTinyGlobal($"Top Left: {TopLeft}", TopLeft + new Vector2(5, -8), DebugGraphicsColor);
			MDraw.End();

			MDraw.SpriteBatch.Begin(transformMatrix: translationMatrix * displayProperties.TransformMatrix,
				samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
			Array.ForEach(Glyphs, x => x.Render());
			if (displayProperties.OverflowBehavior == OverflowBehavior.Wrap) {
				MDraw.DrawRayGlobal(new Vector2(displayProperties.MaxWidth, 0), new Vector2(0, 1000), DebugGraphicsColor);
			}
			MDraw.End();
		}

		public override string ToString() => $"Typograph ({PlainText})";

	}

}