using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova {

	public class Solid {

		public Vector2 remainder;
		public Point Position;

		public bool Collidable { get; set; }

		/// <summary>
		/// Attempt to move the actor by vector amount. If an object is hit, movement is cancelled and onCollide is called.
		/// </summary>
		public void Move(Vector2 amount, IEnumerable<Actor> actorlist) {

			if (!Collidable) {
				// No collisions possible. Just move.
				Position += amount.RoundToPoint();
				return;
			}

			remainder += amount;

			int moveAmountX = Calc.Round(remainder.X);
			int xDir = Math.Sign(moveAmountX);
			moveAmountX = Math.Abs(moveAmountX);

			int moveAmountY = Calc.Round(remainder.Y);
			int yDir = Math.Sign(moveAmountY);
			moveAmountY = Math.Abs(moveAmountY);

			Console.WriteLine("moving {0} {1}", moveAmountX * xDir, moveAmountY * yDir);
			remainder -= Calc.Round(amount);

			var isRiding = actorlist.Where((x) => x.IsRiding(this));

			Collidable = false;

			if (moveAmountX == 0) {

				// No X movement. Move in Y immediately.

				foreach (var actor in actorlist) {

					if (CheckOverlap(actor)) {
						// Push actor. Crush if collision.
						actor.Move(new Vector2(0, moveAmountY * yDir), actor.Crush);

					} else if (actor.IsRiding(this)) {
						// Carry the actor
						actor.Move(new Vector2(0, moveAmountY * yDir), null);
					}

					Position.Y += yDir * moveAmountY;

				}

				Position.Y += yDir;

			} else {

				// Both X and Y movement or just X movement. Move incrementally in both x and y to move in a roughly straight line.

				float slope = (float)moveAmountY / moveAmountX;
				int x = 0;
				int y = 0;

				while (x < moveAmountX) {

					Console.Write("x");

					foreach (var actor in actorlist) {

						if (CheckOverlap(actor)) {
							// Push the actor. Crush if collision.
							actor.Move(new Vector2(xDir, 0), actor.Crush);
						} else if (actor.IsRiding(this)) {
							// Carry the actor.
							actor.Move(new Vector2(xDir, 0), null);
						}

					}

					Position.X += xDir;

					while (y < x * slope) {

						Console.Write("y");

						foreach (var actor in actorlist) {

							if (CheckOverlap(actor)) {
								// Push actor. Crush if collision.
								actor.Move(new Vector2(0, yDir), actor.Crush);

							} else if (actor.IsRiding(this)) {
								// Carry the actor
								actor.Move(new Vector2(0, yDir), null);
							}

						}

						Position.Y += yDir;

					}

				}

				Console.WriteLine();

			}

			Collidable = true;

		}

		protected virtual bool CheckOverlap(Actor actor) {
			return true;
		}

	}

}