using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui.Text {

	public class FontString : List<FontCharacter> {

		public FontString(IEnumerable<FontCharacter> chars) :
			base(chars) {
		}

		public FontString() { }

		public void Add(FontString fontstring) {
			AddRange(fontstring);
		}

		public static FontString operator +(FontString left, FontString right) {
			return new FontString() { left, right };
		}

		public FontString SubString(int startIndex, int count) {
			List<FontCharacter> chars = new List<FontCharacter>();
			for (int i = 0; i < count; i++) {
				chars.Add(this[startIndex + i]);
			}
			return new FontString(chars);
		}

		public int GetWidth(Font font) {

			int width = 0;

			for (int i = 0; i < Count; i++) {

				width += this[i].XAdvance;
				if (i < Count - 1) {
					width += font.GetKerning(this[i].Character, this[i + 1].Character);
				}

			}

			return width;

		}

		public override string ToString() {
			return string.Concat(this.Select(x => x.Character));
		}

	}

	public class FontMultistring : List<FontString> {

		public FontMultistring(IEnumerable<FontString> strings) :
			base(strings) {
		}

		public void Add(IEnumerable<FontCharacter> line) {
			base.Add(new FontString(line));
		}

		public FontMultistring() { }

		public override string ToString() {
			return string.Join("\n", this);
		}

	}

}