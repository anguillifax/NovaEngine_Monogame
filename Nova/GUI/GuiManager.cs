using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Gui {

	public class StateManager {

		public GuiElement Current { get; protected set; }
		public GuiElement Next { get; protected set; }

		public Dictionary<Type, GuiElement> States;

		public StateManager(params GuiElement[] states) {

			States = states.ToDictionary((x) => x.GetType());

			Console.WriteLine("Generated states dictionary:");
			Console.WriteLine(States.ToPrettyString());

		}

		public void Update() {

			if (Current == null) {

				if (Next != null) {
					// Transition into next state immediately
					Next.OnEnter();
					Current = Next;

				} else {

					Console.WriteLine("No current state!");
					return;

				}

			}

			Current.Update();

			if (Next != null) {
				// StateManager is attempting to transition to a new state.

				if (Current.IsExitTransitionFinished()) {
					// Advance to next state.
					Current = Next;
					Next = null;
				}

			}

			// Check if a transition needs to be triggered.
			Type next = Current.GetNext();
			if (next != null) {

				if (States.ContainsKey(next)) {
					// Set the next element to transition to.
					Next = States[next];
					Current.OnExit();
				} else {
					Console.WriteLine($"WARNING: Given type {next} not in state dictionary!");
				}

			}

		}

	}

	public static class GuiManager {

		public static StateManager StateManager { get; private set; }

		static TestGuiElement test;

		internal static void Init() {
			test = new TestGuiElement();
		}

		internal static void Update() {

			//StateManager.Update();
			test.Resize();
			test.Update();

		}

		internal static void Draw() {
			MDraw.Begin();
			test.Draw();
			MDraw.End();
		}

	}

}