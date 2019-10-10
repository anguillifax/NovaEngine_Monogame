using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova.PhysicsEngine {

	public static class Physics {

		public static readonly Color ColliderDrawColor = Color.LimeGreen;
		public static bool DebugDrawColliders = true;

		public static float Gravity { get; set; }

		public static readonly List<Collider> AllColliders = new List<Collider>();

		public static readonly List<Actor> AllActors = new List<Actor>();
		public static readonly List<Solid> AllSolids = new List<Solid>();

		public static void Update() {

			for (int i = 0; i < AllColliders.Count; i++) {

				for (int j = i + 1; j < AllColliders.Count; j++) {

					if (AllColliders[i].Collide(AllColliders[j])) {
						Console.WriteLine("Collided {0} with {1}", AllColliders[i], AllColliders[j]);
					}

				}

			}

		}

		private static void ResolvePhase() {
			Console.WriteLine("Resolve phase");
		}

	}

}