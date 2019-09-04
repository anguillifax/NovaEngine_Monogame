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

		public static readonly VirtualButton Pause;
		public static readonly VirtualButton Enter;
		public static readonly VirtualButton Clear;
		public static readonly VirtualButton Jump;
		public static readonly VirtualButton Attack;
		public static readonly VirtualButton Unleash;
		public static readonly VirtualButton Restart;

		public static readonly VirtualAxis Horizontal;
		public static readonly VirtualAxis Vertical;

		public static readonly List<VirtualButton> AllButtons = new List<VirtualButton>();

		static InputManager() {
			Pause = new VirtualButton("pause", Keys.Escape, Buttons.Start);
			Enter = new VirtualButton("enter", Keys.Enter, Buttons.A);
			Clear = new VirtualButton("clear", Keys.Back, Buttons.Back);
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

	}

}