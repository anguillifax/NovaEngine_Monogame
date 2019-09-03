using Microsoft.Xna.Framework.Input;
using Project.Input;
using System;

namespace Project {

	public static class InputManager {

		public static event Action InputUpdate;

		public static DebugButton Quit = new DebugButton(Keys.F4);
		public static DebugButton TestLoadBindings = new DebugButton(Keys.F1);
		public static DebugButton TestLoadBindingsDef = new DebugButton(Keys.F3);
		public static DebugButton TestSaveBindings = new DebugButton(Keys.F2);

		public static DebugButton OpenPanel = new DebugButton(Keys.F10);
		public static DebugButton Cycle = new DebugButton(Keys.F11);
		public static DebugButton ClosePanel = new DebugButton(Keys.F12);
		private static bool isRebindPanelOpen;
		private static int rebindPointer = 0;

		public static VirtualButton Pause;
		public static VirtualButton Enter;
		public static VirtualButton Jump;
		public static VirtualButton Attack;
		public static VirtualButton Unleash;
		public static VirtualButton Restart;

		private static VirtualButton[] AllButtons;

		static InputManager() {
			CreateVirtualInputs();
			InputBindingsManager.CreateDefaultBindings();
			AllButtons = new VirtualButton[] {
				Pause, Enter, Jump, Attack, Unleash, Restart
			};
		}

		public static void Update() {
			InputUpdate?.Invoke();

			if (OpenPanel.JustPressed) {
				isRebindPanelOpen = !isRebindPanelOpen;
				if (isRebindPanelOpen) {
					rebindPointer = 0;
				}
				Console.WriteLine("Now binding: " + AllButtons[rebindPointer].Name);
			}

			if (isRebindPanelOpen) {
				if (Cycle.JustPressed) {
					if (rebindPointer >= AllButtons.Length - 1) {
						rebindPointer = 0;
					} else {
						rebindPointer++;
					}
					Console.WriteLine("Now binding: " + AllButtons[rebindPointer].Name);
				}
				RebindingManager.Target = AllButtons[rebindPointer];
				RebindingManager.Update();
			}

			if (Enter.JustPressed) Console.WriteLine("Enter");
		}

		public static void LoadBindings() {
			InputBindingsManager.Load();
		}

		public static void LoadDefaultBindings() {
			InputBindingsManager.LoadDefault();
		}

		public static void SaveBindings() {
			InputBindingsManager.Save();
		}

		private static void CreateVirtualInputs() {
			Pause = new VirtualButton("pause", Keys.Escape, Buttons.Start);
			Enter = new VirtualButton("enter", Keys.Enter, Buttons.A);
			Jump = new VirtualButton("jump");
			Attack = new VirtualButton("attack");
			Unleash = new VirtualButton("unleash");
			Restart = new VirtualButton("restart");
		}

	}

}