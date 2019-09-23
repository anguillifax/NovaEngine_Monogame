using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class Actor {

		public Point Position;

		public Vector2 remainder = new Vector2();

		public virtual bool IsRiding(Solid solid) {
			return true;
		}

		public virtual void Crush() {
			Console.WriteLine("destroyed actor");
		}

		/// <summary>
		/// Attempt to move the actor by vector amount. If an object is hit, movement is cancelled and onCollide is called.
		/// </summary>
		public void Move(Vector2 amount, Action onCollide) {
			remainder += amount;

			int moveAmountX = Calc.Round(remainder.X);
			int xDir = Math.Sign(moveAmountX);
			moveAmountX = Math.Abs(moveAmountX);

			int moveAmountY = Calc.Round(remainder.Y);
			int yDir = Math.Sign(moveAmountY);
			moveAmountY = Math.Abs(moveAmountY);

			remainder -= Calc.Round(amount);

			Console.WriteLine("moving {0} {1}", moveAmountX * xDir, moveAmountY * yDir);

			if (moveAmountX == 0) {
				Console.WriteLine("Y only");
				// No X movement. Only move in Y.
				while (moveAmountY > 0) {
					--moveAmountY;
					if (CanMoveInto(Position + new Point(0, yDir))) {
						Position.Y += yDir;
					} else {
						onCollide?.Invoke();
						break;
					}
				}

			} else {
				// Both X and Y movement or just X movement. Move incrementally in both x and y to move in a roughly straight line.

				float slope = (float)moveAmountY / moveAmountX;
				int x = 0;
				int y = 0;

				while (x < moveAmountX) {

					Console.Write("x");
					if (CanMoveInto(Position + new Point(xDir, 0))) {
						++x;
						Position.X += xDir;
					} else {
						onCollide?.Invoke();
						return;
					}

					while (y < x * slope) {
						Console.Write("y");
						if (CanMoveInto(Position + new Point(0, yDir))) {
							++y;
							Position.Y += yDir;
						} else {
							onCollide?.Invoke();
							return;
						}

					}

				}
				Console.WriteLine();

			}

		}

		public bool CanMoveInto(Point position) {
			return position.X <= 7 && position.Y <= 5;
		}

	}

}