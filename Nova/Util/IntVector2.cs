using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	[Serializable]
	public struct IntVector2 : IEquatable<IntVector2> {

		public int X { get; set; }
		public int Y { get; set; }

		public static readonly IntVector2 Zero = new IntVector2(0, 0);

		public static readonly IntVector2 Right = new IntVector2(1, 0);
		public static readonly IntVector2 Left = new IntVector2(-1, 0);
		public static readonly IntVector2 Up = new IntVector2(0, 1);
		public static readonly IntVector2 Down = new IntVector2(0, -1);
		public static readonly IntVector2 UpRight = new IntVector2(1, 1);
		public static readonly IntVector2 One = UpRight;
		public static readonly IntVector2 UpLeft = new IntVector2(-1, 1);
		public static readonly IntVector2 DownRight = new IntVector2(1, -1);
		public static readonly IntVector2 DownLeft = new IntVector2(-1, -1);

		#region Constructors

		/// <summary>
		/// Set both X and Y to value
		/// </summary>
		public IntVector2(int value) :
			this(value, value) {
		}

		/// <summary>
		/// Set X and Y components
		/// </summary>
		public IntVector2(int x, int y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Round and set X and Y components
		/// </summary>
		public IntVector2(float x, float y) :
			this(Calc.RoundToInt(x), Calc.RoundToInt(y)) {
		}

		/// <summary>
		/// Use rounded Vector2 to set X and Y components
		/// </summary>
		public IntVector2(Vector2 v) :
			this(v.X, v.Y) {
		}

		/// <summary>
		/// Set X and Y components from point
		/// </summary>
		public IntVector2(Point p) :
			this(p.X, p.Y) {
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		public IntVector2(IntVector2 other) :
			this(other.X, other.Y) {
		}

		#endregion

		public Point ToPoint() {
			return new Point(X, Y);
		}

		public Vector2 ToVector2() {
			return new Vector2(X, Y);
		}

		public override bool Equals(object obj) {
			return Equals((IntVector2)obj);
		}

		public override int GetHashCode() {
			return 10000 * X + Y;
		}

		public bool Equals(IntVector2 other) {
			return base.Equals(other);
		}

		public override string ToString() {
			return string.Format("({0}, {1})", X, Y);
		}

		/// <summary>
		/// Returns taxicab distance
		/// </summary>
		public static int Distance(IntVector2 from, IntVector2 to) {
			return Math.Abs(to.X - from.X) + Math.Abs(to.Y - from.Y);
		}

		#region Operators

		public static bool operator ==(IntVector2 v1, IntVector2 v2) {
			return v1.Equals(v2);
		}

		public static bool operator !=(IntVector2 v1, IntVector2 v2) {
			return !v1.Equals(v2);
		}

		public static IntVector2 operator +(IntVector2 v1, IntVector2 v2) {
			return new IntVector2(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static IntVector2 operator -(IntVector2 v1, IntVector2 v2) {
			return new IntVector2(v1.X - v2.X, v1.Y - v2.Y);
		}

		public static IntVector2 operator *(IntVector2 v1, IntVector2 v2) {
			return new IntVector2(v1.X * v2.X, v1.Y * v2.Y);
		}

		public static IntVector2 operator /(IntVector2 v1, IntVector2 v2) {
			return new IntVector2(v1.X / v2.X, v1.Y / v2.Y);
		}

		public static IntVector2 operator %(IntVector2 v1, IntVector2 v2) {
			return new IntVector2(v1.X % v2.X, v1.Y % v2.Y);
		}

		public static IntVector2 operator -(IntVector2 v) {
			return new IntVector2(-v.X, -v.Y);
		}

		#endregion

		#region Int Operators

		public static IntVector2 operator *(int i, IntVector2 v) {
			return new IntVector2(i * v.X, i * v.Y);
		}

		public static IntVector2 operator *(IntVector2 v, int i) {
			return i * v;
		}

		public static IntVector2 operator /(IntVector2 v, int i) {
			return new IntVector2(v.X / i, v.Y / i);
		}

		public static IntVector2 operator %(int i, IntVector2 v) {
			return new IntVector2(v.X % i, v.Y % i);
		}

		public static IntVector2 operator %(IntVector2 v, int i) {
			return i % v;
		}

		#endregion

	}

}