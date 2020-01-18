using Microsoft.Xna.Framework;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nova.Gui.GText {

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

	public class Typograph {

		private static readonly Regex RegexWrap = new Regex(@"—*[^\s—]+—*|—+|[^\S\n]+|\n");
		private static readonly Regex RegexNewLine = new Regex(@"\n|[^\n]+");


		private Glyph[] Glyphs { get; }

		public Font Font { get; }

		public TypographData Data { get; }
		public string PlainText => Data.PlainText;

		public Vector2 TopLeft { get; set; }
		private TypographDisplayProperties DisplayProperties { get; set; }


		public Typograph(Font font, TypographData data, Vector2 topLeft, TypographDisplayProperties displayProperties = null) {

			Font = font ?? throw new ArgumentNullException("Font cannot be null");
			Data = data ?? throw new ArgumentNullException("String cannot be null");

			TopLeft = topLeft;

			Glyphs = new Glyph[PlainText.Length];

			DisplayProperties = displayProperties ?? new TypographDisplayProperties(OverflowBehavior.Extend);
			PositionGlyphs(true);
			ApplySpans();

		}

		public void Redraw(TypographDisplayProperties properties) {
			DisplayProperties = properties;
			PositionGlyphs(false);
			ApplySpans();
		}

		private void PositionGlyphs(bool createNew) {

			Vector2 pos = Vector2.Zero;
			afterNewLine = true; // Only assignment outside of ConfigureWord()

			if (DisplayProperties.OverflowBehavior == OverflowBehavior.Extend) {

				foreach (Match match in RegexNewLine.Matches(PlainText)) {
					PositionSubSequence(ref pos, createNew, match.Value, match.Index);
				}

			} else {
				// Wrap Mode

				IEnumerable<NonBreakingSequence> nbsSpans = Data.Spans.OfType<NonBreakingSequence>().Where((x) => x.Length > 0);
				//Console.WriteLine($"Found {nbsSpans.Count()} elements: {nbsSpans.ToPrettyString()}");

				MatchCollection matches = RegexWrap.Matches(PlainText);
				for (int i = 0; i < matches.Count; ++i) {

					Match match = matches[i];

					NonBreakingSequence nbs = nbsSpans
						.Where((n) => n.StartIndex < match.Index + match.Length && match.Index + match.Length <= n.StopIndex)
						.OrderByDescending((x) => x.Length)
						.FirstOrDefault();

					if (nbs != null) {
						string preWord = PlainText.Substring(match.Index, nbs.StartIndex - match.Index);

						//Console.WriteLine($"NBS: {nbs}");

						if (preWord.Length > 0) {
							//Console.WriteLine($"  Preword: {preWord.GetRepresentation()}");
							PositionSubSequence(ref pos, createNew, preWord, match.Index);
						}

						string nbsSubstring = PlainText.Substring(nbs.StartIndex, nbs.Length);
						foreach (Match nbsSubLine in RegexNewLine.Matches(nbsSubstring)) {
							//Console.WriteLine($"  SubNBS: {nbsSubLine.Value.GetRepresentation()}");
							PositionSubSequence(ref pos, createNew, nbsSubLine.Value, nbs.StartIndex + nbsSubLine.Index);
						}

						while (i < matches.Count) {
							var m = matches[i];
							//Console.WriteLine($"  Examining {m.Value}");
							if (m.Index + m.Length > nbs.StopIndex) {
								string postWord = PlainText.Substring(nbs.StopIndex, m.Index + m.Length - nbs.StopIndex);
								if (postWord.Length > 0) {
									//Console.WriteLine($"  PostWord: {postWord.GetRepresentation()}");
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

				g.Position = wordPos;
				word.Add(g);

				wordPos.X += g.Data.XAdvance;
				if (startIndex + i > 0) {
					wordPos.X += Font.GetKerning(PlainText[startIndex + i - 1], PlainText[startIndex + i]);
				}

			}

			float wordVisibleEdge = word[word.Count - 1].Position.X + word[word.Count - 1].Data.DrawRect.Width;
			if (DisplayProperties.OverflowBehavior == OverflowBehavior.Wrap && !afterNewLine
				&& pos.X + wordVisibleEdge > DisplayProperties.MaxWidth && !string.IsNullOrWhiteSpace(text)) {

				InsertNewLine(ref pos);
			}

			if (wordPos.X > DisplayProperties.MaxWidth) {
				float posOffset = 0;

				for (int i = 0; i < text.Length; ++i) {

					float charVisibleEdge = word[i].Position.X + posOffset + word[i].Data.DrawRect.Width;
					if (charVisibleEdge > DisplayProperties.MaxWidth) {
						posOffset = -word[i].Position.X;
						if (i > 0) {
							InsertNewLine(ref pos);
						}
					}

					word[i].Position += TopLeft + pos + new Vector2(posOffset, 0);
					if (createNew) Glyphs[startIndex + i] = word[i];
				}

			} else {

				for (int i = 0; i < text.Length; ++i) {
					word[i].Position += TopLeft + pos;
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

		private void ApplySpans() {

			for (int i = 0; i < PlainText.Length; ++i) {
				Glyphs[i].Color = Color.MonoGameOrange;
			}

		}

		public void Update() {

		}

		public void Render() {
			MDraw.Begin();
			foreach (var g in Glyphs) {
				g.Render();
			}
			if (DisplayProperties.OverflowBehavior == OverflowBehavior.Wrap) {
				MDraw.DrawRayGlobal(TopLeft + new Vector2(DisplayProperties.MaxWidth, 0), new Vector2(0, 1000), Color.DimGray);
			}
			MDraw.End();
		}

		public override string ToString() {
			return $"Typograph ({PlainText})";
		}

	}

}