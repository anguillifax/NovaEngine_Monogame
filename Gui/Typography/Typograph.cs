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

		private Glyph[] Glyphs { get; }

		/// <summary>
		/// Returns true if this is a managed typograph with an associated library.
		/// </summary>
		public bool AttachedToLibrary { get; }

		/// <summary>
		/// The library containing this typograph. Returns null if this is an unmanaged typograph.
		/// </summary>
		public Library Library { get; }

		/// <summary>
		/// The localization containing this typograph. Returns null if this is an unmanaged typograph.
		/// </summary>
		public Localization Localization { get; }

		/// <summary>
		/// The font used if a font span cannot be resolved.
		/// </summary>
		public Font FallbackFont => AttachedToLibrary ? Library.DefaultFont : localFont;
		private readonly Font localFont;

		private readonly Color localColor;
		private Color BaseColor => AttachedToLibrary ? Library.DefaultTextColor : localColor;

		private List<Span> spans;
		private List<Token> tokens;
		private readonly IEnumerable<UpdateSpan> updateSpans;

		/// <summary>
		/// The unformatted text displayed by this typograph.
		/// </summary>
		public string PlainText { get; private set; }

		private Vector2 topLeft_;
		public Vector2 TopLeft {
			get => topLeft_;
			set {
				topLeft_ = value;
				translationMatrix.Translation = new Vector3(topLeft_, 0);
			}
		}

		private Matrix translationMatrix;

		private TypographDisplayProperties displayProperties;



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

			spans = new List<Span>();
			tokens = new List<Token>();

			ResolveReferences(data);

			updateSpans = spans.OfType<UpdateSpan>();
			Glyphs = new Glyph[PlainText.Length];
			PositionGlyphs(true);
			InitializeSpans();

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

			spans = new List<Span>();
			tokens = new List<Token>();

			ResolveReferences(data);

			updateSpans = spans.OfType<UpdateSpan>();
			Glyphs = new Glyph[PlainText.Length];
			PositionGlyphs(true);
			InitializeSpans();
		}

		/// <summary>
		/// Recalculate layout of characters based on new display options.
		/// </summary>
		public void Redraw(TypographDisplayProperties properties) {
			displayProperties = properties;
			PositionGlyphs(false);
		}

		private void ResolveReferences(TypographData original) {
			StringBuilder combinedText = new StringBuilder();
			combinedText.Append(original.PlainText);
			spans.AddRange(original.Spans);
			tokens.AddRange(original.Tokens);
			//RecursiveResolve(original, 0, ref combinedText, ref spans, ref tokens, 0);
			PlainText = combinedText.ToString();

			Console.WriteLine(PlainText);
			Console.WriteLine();
			Console.WriteLine(spans.ToIndentString("Final Spans"));
			Console.WriteLine();
			Console.WriteLine(tokens.ToIndentString("Final Tokens"));
			Console.WriteLine();
		}

		private void WriteIndent(int level, string text) {
			for (int i = 0; i < level; ++i) {
				Console.Write("     ");
			}
			Console.WriteLine($"{level}| {text}");
		}

		private void RecursiveResolve(TypographData data, int textOffsetToEnclosing, ref StringBuilder enclosingText, ref List<Span> enclosingSpans, ref List<Token> enclosingTokens, int depth) {

			WriteIndent(depth, "=== Resolving ===");
			WriteIndent(depth, $"Depth: {depth}");
			WriteIndent(depth, data.ToString());

			StringBuilder localPlainText = new StringBuilder(data.PlainText);
			WriteIndent(depth, $"Local base text: {localPlainText}");

			var localSpans = new List<Span>(data.Spans.SortedCopy());
			var localTokens = new List<Token>(data.Tokens.SortedCopy());

			// Resolve style spans

			int spanOffset = 0;
			for (int i = 0; i < data.Spans.Count; ++i) {

				if (data.Spans[i] is StyleSpan style) {

					if (data.AttachedToLibrary) {

						SpanCollection sc = Localization.GetStyle(style.Key);
						WriteIndent(depth, $"  Found style: {style} -> {sc}");

						var elements = new List<Span>(sc.SortedCopy());
						elements.ForEach(x => x.StartIndex = style.StartIndex);
						elements.ForEach(x => x.Length = style.Length);

						localSpans.RemoveAt(i + spanOffset);
						localSpans.InsertRange(i + spanOffset, elements);
						spanOffset += elements.Count - 1;

					} else {

						WriteIndent(depth, "[Warning] Unmanaged typograph cannot have style spans. Ignoring style span.");

					}

				}

			}

			int tokenOffset = 0;
			for (int i = 0; i < data.Tokens.Count; ++i) {

				if (localTokens[i + tokenOffset] is ExternalSymbolToken symbol) {

					string text;

					if (data.AttachedToLibrary) {
						text = Localization.GetExternalSymbol(symbol.Key);
					} else {
						WriteIndent(depth, "[Warning] Unmanaged typograph cannot have external tokens. Using symbol name as value.");
						text = symbol.Key;
					}

					localPlainText.Insert(symbol.Index, text);
					localTokens.RemoveAt(i + tokenOffset);
					--tokenOffset;
					CorrectIndices(ref localSpans, ref localTokens, symbol.Index, text.Length, i);
					WriteIndent(depth, $"  esym {symbol}");

				} else if (localTokens[i + tokenOffset] is InsertionToken insertion) {

					if (data.AttachedToLibrary) {
						TypographData td = Localization.GetInsertion(insertion.Key);
						localTokens.RemoveAt(i + tokenOffset);

						if (td != null) {

							WriteIndent(depth, $"Preparing to recurse '{insertion.Key}'...");

							RecursiveResolve(td, insertion.Index, ref localPlainText, ref localSpans, ref localTokens, depth + 1);

						} else {
							WriteIndent(depth, $"[Warning] Failed to resolve insertion '{insertion.Key}'. Ignoring Insertion token.");
						}

						--tokenOffset;

					} else {
						WriteIndent(depth, "[Warning] Unmanaged typograph cannot have insertion tokens. Ignoring insertion token.");
						localTokens.RemoveAt(i + tokenOffset);
						--tokenOffset;
					}

				}

			}

			// Convert elements to enclosing indices and merge into enclosing shared lists

			localSpans.ForEach(x => x.StartIndex += tokenOffset);
			localTokens.ForEach(x => x.Index += tokenOffset);

			CorrectIndices(ref enclosingSpans, ref enclosingTokens, tokenOffset, localPlainText.Length, 0);

			WriteIndent(depth, $"Offset to enclosing text: {textOffsetToEnclosing}");
			enclosingText.Insert(textOffsetToEnclosing, localPlainText.ToString());

			WriteIndent(depth, $"Offset to enclosing elements: {tokenOffset}");
			var spanPos = enclosingSpans.FindLastIndex(x => x.StartIndex < textOffsetToEnclosing);
			enclosingSpans.InsertRange(tokenOffset, localSpans);
			enclosingTokens.InsertRange(tokenOffset, localTokens);

			WriteIndent(depth, "=== Finished Resolving ===\n");

		}

		private void CorrectIndices(ref List<Span> spans, ref List<Token> tokens, int index, int length, int curTokenIndex = 0) {

			foreach (var span in spans) {

				if (span.StartIndex < index && span.StopIndex > index) {
					// Resolution: Span includes.
					span.Length += length;

				} else if (span.StartIndex == index && span.Length == 0) {
					// Resolution: Targeted span.
					span.Length = length;

				} else if (span.StartIndex >= index) {
					// Resolution: Non targeted span.
					span.StartIndex += length;
				}

			}

			for (int i = curTokenIndex; i < tokens.Count; ++i) {
				if (tokens[i].Index >= index) {
					tokens[i].Index += length;
				}
			}

		}

		private void PositionGlyphs(bool createNew) {

			Vector2 pos = Vector2.Zero;
			afterNewLine = true; // Only assignment outside of ConfigureWord()

			if (displayProperties.OverflowBehavior == OverflowBehavior.Extend) {

				foreach (Match match in RegexNewLine.Matches(PlainText)) {
					PositionSubSequence(ref pos, createNew, match.Value, match.Index);
				}

			} else {
				// Wrap Mode

				IEnumerable<NonBreakingSequenceSpan> nbsSpans = spans.OfType<NonBreakingSequenceSpan>();

				MatchCollection matches = RegexWord.Matches(PlainText);
				for (int i = 0; i < matches.Count; ++i) {

					Match match = matches[i];

					NonBreakingSequenceSpan nbs = nbsSpans
						.Where((n) => n.StartIndex < match.Index + match.Length && match.Index + match.Length <= n.StopIndex)
						.OrderByDescending((x) => x.Length)
						.FirstOrDefault();

					if (nbs != null) {
						string preWord = PlainText.Substring(match.Index, nbs.StartIndex - match.Index);

						if (preWord.Length > 0) {
							PositionSubSequence(ref pos, createNew, preWord, match.Index);
						}

						string nbsSubstring = PlainText.Substring(nbs.StartIndex, nbs.Length);
						foreach (Match nbsSubLine in RegexNewLine.Matches(nbsSubstring)) {
							PositionSubSequence(ref pos, createNew, nbsSubLine.Value, nbs.StartIndex + nbsSubLine.Index);
						}

						while (i < matches.Count) {
							var m = matches[i];
							if (m.Index + m.Length > nbs.StopIndex) {
								string postWord = PlainText.Substring(nbs.StopIndex, m.Index + m.Length - nbs.StopIndex);
								if (postWord.Length > 0) {
									PositionSubSequence(ref pos, createNew, postWord, nbs.StopIndex);
								}
								break;
							}
							++i;
						}

					} else {

						PositionSubSequence(ref pos, createNew, match.Value, match.Index);
						afterNewLine = match.Value == "\n";

					}

				}

			}

		}

		/// <summary>
		/// Records whether or not the previously written word was a LF new line character.
		/// <para>Persistent variable for use of ConfigureWord() only.</para>
		/// </summary>
		private bool afterNewLine;

		private void PositionSubSequence(ref Vector2 pos, bool createNew, string text, int startIndex) {

			if (text == "\n") {
				pos.X = 0;
				pos.Y += FallbackFont.LineHeight;
				if (createNew) Glyphs[startIndex] = FallbackFont.GetGlyph('\n');
				afterNewLine = true;
				return;
			}

			var word = new List<Glyph>();

			Vector2 wordPos = Vector2.Zero;

			for (int i = 0; i < text.Length; ++i) {

				Glyph g = createNew ? FallbackFont.GetGlyph(text[i]) : Glyphs[startIndex + i];

				g.CharacterPosition = wordPos;
				word.Add(g);

				wordPos.X += g.Data.XAdvance;
				if (startIndex + i > 0) {
					wordPos.X += FallbackFont.GetKerning(PlainText[startIndex + i - 1], PlainText[startIndex + i]);
				}

			}

			float wordVisibleEdge = word[word.Count - 1].CharacterPosition.X + word[word.Count - 1].Data.DrawRect.Width;
			if (displayProperties.OverflowBehavior == OverflowBehavior.Wrap && !afterNewLine
				&& pos.X + wordVisibleEdge > displayProperties.MaxWidth && !string.IsNullOrWhiteSpace(text)) {

				InsertNewLine(ref pos);
			}

			if (wordPos.X > displayProperties.MaxWidth) {
				float posOffset = 0;

				for (int i = 0; i < text.Length; ++i) {

					float charVisibleEdge = word[i].CharacterPosition.X + posOffset + word[i].Data.DrawRect.Width;
					if (charVisibleEdge > displayProperties.MaxWidth) {
						posOffset = -word[i].CharacterPosition.X;
						if (i > 0) {
							InsertNewLine(ref pos);
						}
					}

					word[i].CharacterPosition += pos + new Vector2(posOffset, 0);
					if (createNew) Glyphs[startIndex + i] = word[i];
				}

			} else {

				for (int i = 0; i < text.Length; ++i) {
					word[i].CharacterPosition += pos;
					if (createNew) Glyphs[startIndex + i] = word[i];
				}

			}

			pos += wordPos;
			afterNewLine = false;

		}

		private void InsertNewLine(ref Vector2 pos) {
			pos.X = 0;
			pos.Y += FallbackFont.LineHeight;
		}

		private void InitializeSpans() {

			Array.ForEach(Glyphs, (x) => x.Color = BaseColor);

			foreach (var span in spans) {
				span.Initialize(this, GetSliceFromSpan(span));
			}

		}

		public void Update() {

			Array.ForEach(Glyphs, (x) => x.Offset = Vector2.Zero);

			foreach (var span in updateSpans) {
				span.Update(this, GetSliceFromSpan(span));
			}

		}

		private GlyphSequence GetSliceFromSpan(Span span) => new GlyphSequence(Glyphs.Skip(span.StartIndex).Take(span.Length));

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