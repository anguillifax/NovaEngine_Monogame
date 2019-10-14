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

		public static readonly List<BoxCollider> AllColliders = new List<BoxCollider>();

		public static readonly List<Actor> AllActors = new List<Actor>();
		public static readonly List<Solid> AllSolids = new List<Solid>();

		public static void Update() {

			// Broadphase();
			Narrowphase();
			ResolvingPhase();
			// GenerateContacts();

		}

		#region Narrowphase

		private static void Narrowphase() {

			foreach (var actor in AllActors) {

				Vector2 allowedVelocity = GetAllowedVelocity(actor);

				float time = GetEarliestTimeOfImpact(actor, allowedVelocity);

				actor.Entity.Position += allowedVelocity * time;

			}

			foreach (var solid in AllSolids) {

				foreach (var solidCollider in solid.Colliders) {

				}

				solid.Entity.Position += solid.Velocity;

			}
		}

		private static Vector2 GetAllowedVelocity(Actor actor) {

			Vector2 allowedVelocity = actor.Velocity;

			foreach (var collider in AllColliders) {
				if (actor.Colliders.Contains(collider)) continue; // skip self

				foreach (var actorCollider in actor.Colliders) {

					if (PhysicsMath.IsOverlapping(actorCollider, collider)) {

						allowedVelocity = PhysicsMath.GetAllowedVelocity(actor.Velocity, PhysicsMath.GetNormal(actorCollider, collider));

					}

				}

			}

			return allowedVelocity;
		}

		/// <summary>
		/// Run dynamic intersection test against all colliders. Return the earliest time of impact, or t=1f if no collisions occured.
		/// </summary>
		private static float GetEarliestTimeOfImpact(Actor actor, Vector2 allowedVelocity) {

			float time = 1f;

			foreach (var collider in AllColliders) {
				if (actor.Colliders.Contains(collider)) continue; // skip self

				foreach (var actorCollider in actor.Colliders) {

					if (PhysicsMath.IntersectMovingNoOverlap(collider, actorCollider, collider.Velocity, allowedVelocity, out float first)) {

						Console.WriteLine("HIT against {0}", collider);
						time = Math.Min(time, first);
					}

				}

			}

			return time;
		}

		#endregion


		#region Resolving Phase

		private static void ResolvingPhase() {

			foreach (var actor in AllActors) {

				foreach (var collider in AllColliders) {
					if (actor.Colliders.Contains(collider)) continue; // skip self

					if (PhysicsMath.IsInside(actor.Colliders[0], collider)) {
						actor.Entity.Position = PhysicsMath.Depenetrate(actor.Colliders[0], collider);
					}

				}

			}
		}

		#endregion

		public static void Draw() {
			MDraw.Begin();

			foreach (var actor in AllActors) {
				// draw velocity
				MDraw.DrawLine(actor.Entity.Position, actor.Entity.Position + actor.Velocity / Time.DeltaTime, Color.Gray);
				MDraw.DrawPoint(actor.Entity.Position + actor.Velocity / Time.DeltaTime, Color.Gray);
			}

			MDraw.End();
		}

	}

}