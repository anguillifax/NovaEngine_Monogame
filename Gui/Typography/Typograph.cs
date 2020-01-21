using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.Gui.Typography {

	public enum OverflowBehavior {
		Extend, Wrap
	}

	public class TypographDisplayProperties {
		public OverflowBehavior OverflowBehavior { get; set; }
		public float MaxWidth { get; set; }

		public TypographDisplayProperties(OverflowBehavior overflowBehavior, float maxWidth = float.MaxValue) {
			OverflowBehavior = overflowBehavior;
			MaxWidth = maxWidth;
		}

	}

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

		private readonly Font localFont;
		public Font Font => AttachedToLibrary ? Localization.GetFont() : localFont;

		private readonly Color localColor;
		private Color BaseColor => AttachedToLibrary ? Library.DefaultTextColor : localColor;

		private TypographData Data { get; }

		/// <summary>
		/// The unformatted text displayed by this typograph.
		/// </summary>
		public string PlainText { get; private set; }

		public Vector2 TopLeft { get; set; }
		private TypographDisplayProperties displayProperties;

		private readonly IEnumerable<UpdateSpan> updateSpans;

		/// <summary>
		/// Create an unmanaged typograph. Unmanaged typographs cannot contain referential elements, such as typographic insertions, external symbols, or style spans.
		/// </summary>
		public Typograph(TypographData data, Font font, Color baseColor, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {

			Data = data ?? throw new ArgumentNullException("Typograph data cannot be null");
			localFont = font ?? throw new ArgumentNullException("Font cannot be null");

			PlainText = data.PlainText;
			localColor = baseColor;
			TopLeft = topLeft;

			AttachedToLibrary = false;
			Localization = null;
			Library = null;

			this.displayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);
			updateSpans = data.Spans.GetByType<UpdateSpan>();

			ResolveInsertions();
			Glyphs = new Glyph[PlainText.Length];
			PositionGlyphs(true);
			InitializeSpans();

		}

		/// <summary>
		/// Create a managed typograph. Managed typographs have full access to all features of the typographic toolkit.
		/// </summary>
		public Typograph(TypographData data, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {
			Data = data ?? throw new ArgumentNullException("Typograph data cannot be null");

			PlainText = data.PlainText;
			TopLeft = topLeft;

			AttachedToLibrary = true;
			Localization = Data.Localization;
			Library = Data.Localization.Library;

			this.displayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);
			updateSpans = data.Spans.GetByType<UpdateSpan>();

			ResolveInsertions();
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

		private void ResolveInsertions() {

			for (int i = 0; i < Data.Tokens.Count; ++i) {

				if (Data.Tokens[i] is ExternalSymbolToken symbol) {

					if (AttachedToLibrary) {
						string text = Localization.GetExternalSymbol(symbol.Key);
						PlainText = PlainText.Insert(symbol.Index, text);
						Console.WriteLine($"{symbol.Index} : {symbol.Key}");
						CorrectIndicesAfterInsertion(symbol.Index, text.Length, i);

					} else {
						Console.WriteLine("[Warning] Unmanaged typograph cannot have external symbol tokens. Using external symbol key as symbol value.");
						PlainText = PlainText.Insert(symbol.Index, symbol.Key);
						CorrectIndicesAfterInsertion(symbol.Index, symbol.Key.Length, i);
					}

				} else if (Data.Tokens[i] is InsertionToken insertion) {

					if (AttachedToLibrary) {
						//TODO
					} else {
						Console.WriteLine("[Warning] Unmanaged typograph cannot have insertion tokens. Using insertion name as value.");
						string s = "{" + insertion.Key + "}";
					}

				}

			}

		}

		private void CorrectIndicesAfterInsertion(int index, int length, int curTokenIndex) {

			foreach (var span in Data.Spans) {

				if (span.StartIndex < index && span.StopIndex > index) {
					// Resolution: Span includes.
					span.Length += length;

				} else if (span.StartIndex == index && span.Length == 0) {
					// Resolution: Targeted span.
					span.Length = length;

				} else if (span.StartIndex >= index) {
					// Resolution: Non targeted span.
					span.StartIndex += length;
					Console.WriteLine($"{span}: +start {length} -> {span.StopIndex}");
				}

			}

			for (int i = curTokenIndex + 1; i < Data.Tokens.Count; ++i) {
				if (Data.Tokens[i].Index >= index) {
					Data.Tokens[i].Index += length;
					Console.WriteLine($"{Data.Tokens[i]}: +length {length} -> {Data.Tokens[i].Index}");
				}
				Console.WriteLine("--");
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

				IEnumerable<NonBreakingSequenceSpan> nbsSpans = Data.Spans.GetByType<NonBreakingSequenceSpan>();

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
				pos.Y += Font.LineHeight;
				if (createNew) Glyphs[startIndex] = Font.GetGlyph('\n');
				afterNewLine = true;
				return;
			}

			var word = new List<Glyph>();

			Vector2 wordPos = Vector2.Zero;

			for (int i = 0; i < text.Length; ++i) {

				Glyph g = createNew ? Font.GetGlyph(text[i]) : Glyphs[startIndex + i];

				g.CharacterPosition = wordPos;
				word.Add(g);

				wordPos.X += g.Data.XAdvance;
				if (startIndex + i > 0) {
					wordPos.X += Font.GetKerning(PlainText[startIndex + i - 1], PlainText[startIndex + i]);
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
			pos.Y += Font.LineHeight;
		}

		private void InitializeSpans() {

			Array.ForEach(Glyphs, (x) => x.Color = BaseColor);

			foreach (var span in Data.Spans) {
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
			MDraw.Begin();
			foreach (var g in Glyphs) {
				g.Render(TopLeft);
			}
			if (displayProperties.OverflowBehavior == OverflowBehavior.Wrap) {
				MDraw.DrawRayGlobal(TopLeft + new Vector2(displayProperties.MaxWidth, 0), new Vector2(0, 1000), Color.DimGray);
			}
			MDraw.End();
		}

		public override string ToString() => $"Typograph ({PlainText})";

	}

}