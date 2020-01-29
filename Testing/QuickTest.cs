using Microsoft.Xna.Framework;
using Nova.Gui.Typography;
using Nova.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nova {

	public class QuickTest {

		Typograph typograph;
		TypographDisplayProperties displayProperties;

		Library library;
		Localization english, englishUS;

		public void Init() {

			library = new Library("Testing", Color.White, new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBold.fnt"), 32f);
			library.GlobalLocalization.ExternalSymbols.Add("player", () => "Anguillifax(Global)");
			library.GlobalLocalization.Styles.Add("player", new SpanCollection(
					new ColorSpan(ColorUtil.FromHex("FF2222")),
					new JitterSpan(.7f)
				));
			library.GlobalLocalization.Styles.Add("fancy", new SpanCollection(
					new RainbowColorSpan()
				));
			library.GlobalLocalization.Insertions.Add("one", new TypographData("One", library.GlobalLocalization));
			library.GlobalLocalization.Insertions.Add("two", new TypographData("[Two ]", library.GlobalLocalization, 
				//new ColorSpan(0, 6, Color.Goldenrod),
				new InsertionToken(5, "one")
				));
			library.FontTable.Add("rbold", new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBoldItalic.fnt"));
			library.FontTable.Add("rbig", new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBig.fnt"));
			library.FontTable.Add("fsys", new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\FixedSys.fnt"));
			library.FontTable.Add("mc", new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\Minecraftia.fnt"));
			library.FontTable.Add("pdos", new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\PerfectDos.fnt"));

			english = library.CreateLocalization("en-BASE");
			english.ExternalSymbols.Add("player", () => "Anguillifax(en-BASE)");

			englishUS = library.CreateLocalization("en-US", english);
			englishUS.ExternalSymbols.Add("player", () => "Anguillifax(en-US)");

			Console.WriteLine(library.ListAllLocalizations());

		}

		void CreateTypograph() {
			string text = TextUtil.NormalizeLineEnding(File.ReadAllText(@"C:\Users\Bryan\Desktop\text.gtxt"));
			displayProperties = new TypographDisplayProperties(OverflowBehavior.Extend, 500);

			var td = LoadFromMarkup.Load(text, english, false);
			Console.WriteLine(td.ToStringDebug());


			typograph = new Typograph(td, new Vector2(100, 100), displayProperties);
			//typograph = new Typograph(td, library.DefaultFont, library.DefaultTextColor, library.DefaultFontSize, new Vector2(100, 100), displayProperties);
		}

		public void LoadContent() {
			CreateTypograph();

		}

		public void Update() {
			typograph.Update();

			if (InputManager.Any.Jump.JustPressed) {
				CreateTypograph();
			}

			//if (InputManager.Any.Horizontal.Value != 0) {
			//	displayProperties.MaxWidth += InputManager.Any.Horizontal.Value * 2f;
			//	typograph.Redraw(displayProperties);
			//}

			//if (InputManager.Any.Vertical.Value != 0) {
			//	typograph.TopLeft += new Vector2(0, -10 * InputManager.Any.Vertical.Value);
			//	typograph.Redraw(displayProperties);
			//}

		}

		public void Draw() {
			typograph.Render();
		}

	}

}