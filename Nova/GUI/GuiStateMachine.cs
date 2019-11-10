using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.Gui {

	public class GuiStateMachine {

		protected GuiStack Stack { get; }

		public List<GuiElement> AllElements { get; protected set; }

		public GuiStateMachine(params GuiElement[] states) {

			AllElements = new List<GuiElement>(states);
			Stack = new GuiStack();

			Console.WriteLine("All States: " + AllElements.ToPrettyString());

			Engine.OnResolutionChanged += Engine_OnResolutionChanged;

		}

		private void Engine_OnResolutionChanged(object sender, EventArgs e) {
			Stack.Current?.Resize();
		}

		public void ClearAndSetCurrent(GuiElement next) {
			Stack.Clear();
			Stack.Push(next);
			Stack.Current.Resize();
			Stack.Current.OnEnter();
			Console.WriteLine($"CURRENT IS NOW {Stack.Current}");
		}

		public void Update() {

			if (Stack.Count == 0) {
				Console.WriteLine("Stack is empty!");
				return;
			}

			UpdateTransitionToNext();

			if (Stack.Count == 0) {
				Console.WriteLine("Popped last element!");
				return;
			}

			Stack.Current.Update();

			UpdateNextElement();

		}

		private void UpdateTransitionToNext() {

			if (Stack.Count > 1) {
				// There is at least one element in the next queue.

				if (Stack.Current.IsExitTransitionFinished()) {

					Stack.Next.Resize();
					Stack.Next.OnEnter();

					Stack.Pop();
					Console.WriteLine("Stack is now length " + Stack.Count);

					Console.WriteLine($"CURRENT IS NOW {Stack.Current}");

				}

			}

		}

		private void UpdateNextElement() {

			for (int i = 0; i < 1; i++) {

				GuiElement current = Stack[i];

				GuiElement next = current.GetNextElement(AllElements);

				if (Stack.CanPush(next)) {

					// Only call OnExit() if this is the first new element.
					bool invokeExit = Stack.Count == 1;

					Stack.Push(next);
					Console.WriteLine("Stack is now length " + Stack.Count);

					if (invokeExit) {
						Stack.Current.OnExit();
					}

				}

			}

		}

		public void Draw() {
			Stack.Current?.Draw();
		}

	}
}