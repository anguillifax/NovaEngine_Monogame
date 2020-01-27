﻿using Microsoft.Xna.Framework;
using Nova.Gui.Typography;
using Nova.Util;
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


			library = new Library("Testing", Color.MonoGameOrange, new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBold.fnt"));
			library.GlobalLocalization.ExternalSymbols.Add("player", () => "Anguillifax(Global)");
			library.GlobalLocalization.Styles.Add("player", new SpanCollection(
					new ColorSpan(ColorUtil.FromHex("FF2222")),
					new JitterSpan(.7f)
				));
			library.GlobalLocalization.Styles.Add("fancy", new SpanCollection(
					new RainbowColorSpan()
				));
			library.GlobalLocalization.Insertions.Add("one", new TypographData("One", library.GlobalLocalization));
			library.GlobalLocalization.Insertions.Add("two", new TypographData("[Two ]", library.GlobalLocalization, new InsertionToken(5, "one")));
			library.AddFont(new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBoldItalic.fnt"));
			library.AddFont(new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBig.fnt"));

			english = library.CreateLocalization("en-BASE");
			english.ExternalSymbols.Add("player", () => "Anguillifax(en-BASE)");

			englishUS = library.CreateLocalization("en-US", english);
			englishUS.ExternalSymbols.Add("player", () => "Anguillifax(en-US)");

			System.Console.WriteLine(library.ListAllLocalizations());

		}

		public void LoadContent() {
			string text = TextUtil.NormalizeLineEnding(File.ReadAllText(@"C:\Users\Bryan\Desktop\text.gtxt"));
			displayProperties = new TypographDisplayProperties(OverflowBehavior.Extend, 500);

			var td = LoadFromMarkup.Load(text, english);
			System.Console.WriteLine(td.ToStringDebug());


			typograph = new Typograph(td, new Vector2(100, 100), displayProperties);
			//typograph = new Typograph(td, library.DefaultFont, library.DefaultTextColor, new Vector2(100, 100), displayProperties);

		}

		public void Update() {
			typograph.Update();

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
			typograph?.Render();
		}

	}

}