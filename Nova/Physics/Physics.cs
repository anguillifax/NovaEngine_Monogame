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

			//Console.WriteLine();
			//Console.WriteLine();

		}

		#region Narrowphase

		private static void Narrowphase() {

			foreach (var actor in AllActors) {

				Vector2 allowedVelocity = GetAllowedVelocity(actor);

				Console.WriteLine("{0} {1}", actor.Velocity, allowedVelocity);

				Console.WriteLine();

				float time = GetEarliestTimeOfImpact(actor, allowedVelocity);
				//Console.WriteLine("earliest {0}", time);

				actor.Entity.Position += allowedVelocity * time;

			}

			foreach (var solid in AllSolids) {

				foreach (var actor in AllActors) {

					foreach (var solidCollider in solid.Colliders) {

						foreach (var actorCollider in actor.Colliders) {

							//Console.WriteLine($"actor {actorCollider.Position.ToStringHighPrecision()}, solid {solidCollider.Position.ToStringHighPrecision()}");

							if (PhysicsMathExact.IntersectPushFuzzy(solidCollider, actorCollider, solid.Velocity, out Vector2 delta)) {

								//Console.WriteLine($"delta push of {delta.ToStringHighPrecision()} to {(actor.Entity.Position + delta).ToStringHighPrecision()}");
								actor.Entity.Position += delta;

							}

						}

					}
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

						Console.WriteLine("Touching {0}", collider);
						
						allowedVelocity = PhysicsMath.GetAllowedVelocity(allowedVelocity, PhysicsMath.GetNormal(actorCollider, collider));

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

					if (PhysicsMathExact.IntersectMovingNoOverlapFuzzy(collider, actorCollider, collider.Velocity, allowedVelocity, out float first)) {

						//Console.WriteLine("HIT against {0}", collider);
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

					if (PhysicsMathExact.IsInside(actor.Colliders[0], collider)) {
						var newPos = PhysicsMathExact.Depenetrate(actor.Colliders[0], collider);
						Console.WriteLine("Corrected by {0}", (actor.Entity.Position - newPos).ToStringHighPrecision());
						actor.Entity.Position = newPos;
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