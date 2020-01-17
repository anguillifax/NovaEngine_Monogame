using Microsoft.Xna.Framework;
using Nova.Gui.GText;
using System.IO;
using System.Linq;

namespace Nova {

	public class QuickTest {

		Font font;
		string text;

		Typograph typograph;

		public void Init() {
		}

		public void LoadContent() {
			font = new Font(@"C:\Users\Bryan\Desktop\BM Font\RobotoBold.xml");
			text = File.ReadAllText(@"C:\Users\Bryan\Desktop\tmp.txt");
			typograph = new Typograph(font, text, new Vector2(100, 100));
		}

		public void Update() {
			typograph.Update();
		}

		public void Draw() {
			typograph?.Render();

		}

	}

}