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

			// Broadphase();
			Narrowphase();
			ResolvingPhase();
			// GenerateContacts();

		}

		#region Narrowphase

		private static void Narrowphase() {

			foreach (var solid in AllSolids) {

				// push

			}

			foreach (var actor in AllActors) {

				Vector2 allowedVelocity = GetAllowedVelocity(actor);

				float time = GetEarliestTimeOfImpact(actor, allowedVelocity);

				actor.Entity.Position += allowedVelocity * time;

			}
		}

		private static Vector2 GetAllowedVelocity(Actor actor) {

			Vector2 allowedVelocity = actor.Velocity;

			foreach (var collider in AllColliders) {
				if (actor.Colliders.Contains(collider)) continue; // skip self

				if (PhysicsMath.IsOverlapping_Box_Box((BoxCollider)actor.Colliders[0], (BoxCollider)collider)) {

					allowedVelocity = PhysicsMath.GetAllowedVelocity(actor.Velocity, PhysicsMath.GetNormal_Box_Box((BoxCollider)actor.Colliders[0], (BoxCollider)collider));

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

				if (PhysicsMath.IntersectMoving_Box_Box_NoOverlap((BoxCollider)collider, (BoxCollider)actor.Colliders[0], Vector2.Zero, allowedVelocity, out float first)) {

					Console.WriteLine("HIT against {0}", collider);
					time = Math.Min(time, first);
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

					if (PhysicsMath.IsInside_Box_Box((BoxCollider)actor.Colliders[0], (BoxCollider)collider)) {
						actor.Entity.Position = PhysicsMath.Depenetrate_Box_Box((BoxCollider)actor.Colliders[0], (BoxCollider)collider);
					}

				}

			}
		}

		#endregion

		public static void Draw() {
			MDraw.Begin();

			foreach (var actor in AllActors) {
				MDraw.DrawLine(actor.Entity.Position, actor.Entity.Position + actor.Velocity / Time.DeltaTime, Color.Gray);
				MDraw.DrawPoint(actor.Entity.Position + actor.Velocity / Time.DeltaTime, Color.Gray);
				//MDraw.DrawLine(actor.Entity.Position, actor.Entity.Position + allowedVelocity, Color.White);
				//MDraw.DrawPoint(actor.Entity.Position + allowedVelocity, Color.White);
			}
			MDraw.End();
		}

		static bool ContactListContainsCollider(IEnumerable<ContactPoint> points, Collider c) {
			foreach (var item in points) {
				if (item.collider == c) {
					return true;
				}
			}
			return false;
		}

	}

}