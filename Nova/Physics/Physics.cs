using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.PhysicsEngine {

	public static class Physics {

		public static readonly Color ColliderDrawColor = Color.LimeGreen;
		public static bool DebugDrawColliders = true;

		public static float Gravity { get; set; }

		public static readonly List<Collider> AllColliders = new List<Collider>();

		public static readonly List<Actor> AllActors = new List<Actor>();
		public static readonly List<Solid> AllSolids = new List<Solid>();

		public static void Update() {

			foreach (var actor in AllActors) {

				float time = 1f;

				foreach (var box in AllColliders.Cast<BoxCollider>()) {
					// do not process self
					if (actor.Colliders.Contains(box)) continue;

					if (PhysicsMath.IntersectMovingBoxAgainstBox((BoxCollider)actor.Colliders[0], box, actor.Velocity, Vector2.Zero, out float first, out float last)) {
						time = Math.Min(time, first);
						//Console.WriteLine("hit " + first);
					}

				}

				Console.WriteLine($"{actor.Velocity}, t={time}, {actor.Entity.Position + time * actor.Velocity}");

				actor.Entity.Position += actor.Velocity * time;
			}

		}

		private static void CheckOverlap() {

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