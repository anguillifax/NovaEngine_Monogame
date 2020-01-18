using Microsoft.Xna.Framework;
using Nova.Gui.GText;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nova {

	public class QuickTest {

		Font font;
		string text;

		Typograph typograph;
		TypographDisplayProperties displayProperties;


		public void Init() {
		}

		public void LoadContent() {
			font = new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBold.fnt");
			text = File.ReadAllText(@"C:\Users\Bryan\Desktop\tmp.txt");
			displayProperties = new TypographDisplayProperties(OverflowBehavior.Wrap, 200);

			Locale defLocale = new Locale();
			TypographData td = new TypographData(text, defLocale);
			td.Add(new NonBreakingSequence(56, 21));
			//td.Spans.Add(new NonBreakingSequence())
			typograph = new Typograph(font, td, new Vector2(100, 100), displayProperties);

		}

		public void Update() {
			typograph.Update();

			if (InputManager.Any.Horizontal.Value != 0) {
				displayProperties.MaxWidth += InputManager.Any.Horizontal.Value * 2f;
				typograph.Redraw(displayProperties);
			}

			if (InputManager.Any.Vertical.Value != 0) {
				typograph.TopLeft += new Vector2(0, -10 * InputManager.Any.Vertical.Value);
				typograph.Redraw(displayProperties);
			}

		}

		public void Draw() {
			typograph?.Render();
		}

	}

}