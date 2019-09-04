using Microsoft.Xna.Framework.Input;
using Nova.Input;
using System;
using System.Collections.Generic;

namespace Nova {

	public static class InputManager {

		public static event Action InputUpdate;

		public static DebugButton Quit = new DebugButton(Keys.F4);
		public static DebugButton TestLoadBindings = new DebugButton(Keys.F1);
		public static DebugButton TestLoadBindingsDef = new DebugButton(Keys.F3);
		public static DebugButton TestSaveBindings = new DebugButton(Keys.F2);

		public static DebugButton RebindingPanel = new DebugButton(Keys.F10);

		public static VirtualButton Pause;
		public static VirtualButton Enter;
		public static VirtualButton Jump;
		public static VirtualButton Attack;
		public static VirtualButton Unleash;
		public static VirtualButton Restart;

		public static VirtualAxis Horizontal;
		public static VirtualAxis Vertical;

		public static List<VirtualButton> AllButtons = new List<VirtualButton>();

		static InputManager() {
			CreateVirtualInputs();
			InputBindingsManager.CreateDefaultBindings(); // needs button names
		}

		public static void Update() {
			InputUpdate?.Invoke();
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

			Horizontal = new VirtualAxis("horz", Keys.Right, Keys.Left,
				new VirtualAxisInput.StickLeftHorz(),
				new VirtualAxisInput.DPadHorz()
				);

			Vertical = new VirtualAxis("vert", Keys.Up, Keys.Down,
				new VirtualAxisInput.StickLeftVert(),
				new VirtualAxisInput.DPadVert()
				);

		}

	}

}