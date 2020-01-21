using Microsoft.Xna.Framework;
using Nova.Gui.Typography;
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

			library = new Library();
			library.DefaultTextColor = Color.White;
			library.GlobalLocalization.Font = new Font(@"C:\Users\Bryan\Desktop\BM Font\Exports\RobotoBold.fnt");
			library.GlobalLocalization.ExternalSymbols.Add("PlayerNameGlobal", () => "Anguillifax(Global)");

			english = library.CreateLocalization("en-BASE");
			english.ExternalSymbols.Add("PlayerNameEnglish", () => "Anguillifax(en-BASE)");

			englishUS = library.CreateLocalization("en-US", english);
			english.ExternalSymbols.Add("PlayerName", () => "Anguillifax(en-US)");

			library.Test();

		}

		public void LoadContent() {
			string text = GTextUtil.NormalizeLineEnding(File.ReadAllText(@"C:\Users\Bryan\Desktop\tmp.txt"));
			displayProperties = new TypographDisplayProperties(OverflowBehavior.Wrap, 500);

			var td = LoadFromMarkup.Load(text, english);
			System.Console.WriteLine(td.ToStringDebug());


			typograph = new Typograph(td, new Vector2(100, 100), displayProperties);

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