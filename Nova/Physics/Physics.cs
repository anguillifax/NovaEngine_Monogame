using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nova.PhysicsEngine {

	/// <summary>
	/// Used to cache data necessary for physics calculations.
	/// </summary>
	internal struct ProcessingData {
		public Vector2 MoveTo;
		public Vector2 CalcVel;
		public List<Rigidbody> CheckedAgainst; // TODO: optimize with a HashSet
		public float TimeOfImpact;

		public void Reset() {
			if (CheckedAgainst == null) {
				CheckedAgainst = new List<Rigidbody>();
			} else {
				CheckedAgainst.Clear();
			}
		}
	}

	/// <summary>
	/// Central manager of physics simulation.
	/// </summary>
	public static class Physics {

		// Debug properties
		public static readonly Color ColliderDrawColor = Color.LimeGreen;
		public static bool DebugDrawColliders = true;

		// Physic Object Lists
		public static readonly List<BoxCollider> AllColliders = new List<BoxCollider>();
		public static readonly List<ActorRigidbody> AllActors = new List<ActorRigidbody>();
		public static readonly List<SolidRigidbody> AllSolids = new List<SolidRigidbody>();
		public static readonly List<Rigidbody> AllRigidbodies = new List<Rigidbody>();

		// Customization
		public static IVelocityCombinationRuleset Ruleset { get; set; }

		public static Vector2 Gravity { get; set; }


		public static void Update() {

			PreConfigure();

			// Broadphase();
			Narrowphase();
			ResolvingPhase();
			// GenerateContacts();

			Console.WriteLine();
			Console.WriteLine();

		}

		private static void PreConfigure() {

			if (Ruleset == null) {
				Ruleset = new DefaultVelocityCombinationRuleset();
			}

			foreach (var rigidbody in AllRigidbodies) {
				rigidbody.ProcessingData.Reset();
			}
		}

		#region Narrowphase

		private static void Narrowphase() {

			// Get allowed velocity for all solids.
			foreach (var solid in AllSolids) {
				if (IsStationary(solid)) continue;
				CalculateVelocity(solid);
			}

			// Calculate how far the solid can move this update.
			foreach (var solid in AllSolids) {
				if (IsStationary(solid)) continue;
				CalculateEarliestTimeOfImpact(solid);
			}

			// Move the solid, drag attached actors, push actors in movement path, crush sandwiched actors.
			foreach (var solid in AllSolids) {
				if (IsStationary(solid)) continue;
				MoveSolid(solid);
			}

			// Get allowed velocity for all actors.
			foreach (var actor in AllActors) {
				if (actor.Velocity.LengthSquared() == 0f) continue;

				CalculateVelocity(actor);
			}

			// Calculate where to move actors
			foreach (var actor in AllActors) {
				if (IsStationary(actor)) continue;

				float time = GetEarliestTimeOfImpact(actor);

				actor.ProcessingData.MoveTo = actor.Entity.Position + actor.ProcessingData.CalcVel * time;

			}

			// Move actors to calculated position
			foreach (var actor in AllActors) {
				if (IsStationary(actor)) continue;

				actor.Entity.Position = actor.ProcessingData.MoveTo;
			}

		}

		private static void CalculateVelocity(SolidRigidbody solid) {

			solid.ProcessingData.CalcVel = solid.Velocity;

			// TODO: Test this
			foreach (var otherSolid in AllSolids) {

				break;

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				// Skip if pair has already been checked.
				if (solid.ProcessingData.CheckedAgainst.Contains(otherSolid)) continue;


				if (PhysicsMath.IsOverlapping(solid.MainCollider, otherSolid.MainCollider)) {

					if (!PhysicsMath.IsSlidingCorner(solid.ProcessingData.CalcVel, solid.MainCollider, otherSolid.MainCollider)) {

						VelocityPair result = Ruleset.Get(solid, otherSolid);

						Console.WriteLine($"[{solid}] before {solid.ProcessingData.CalcVel}, after {result.left}");
						Console.WriteLine($"[{otherSolid}] before {otherSolid.ProcessingData.CalcVel}, after {result.right}");

						solid.ProcessingData.CalcVel = result.left;
						otherSolid.ProcessingData.CalcVel = result.right;

						solid.ProcessingData.CheckedAgainst.Add(otherSolid);
						otherSolid.ProcessingData.CheckedAgainst.Add(solid);


					}

				}

			}

		}

		private static void CalculateEarliestTimeOfImpact(SolidRigidbody solid) {

			float earliest = 1f;

			// TODO: Test this
			foreach (var otherSolid in AllSolids) {

				break;

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				if (PhysicsMath.IntersectMoving(solid.MainCollider, otherSolid.MainCollider, solid.ProcessingData.CalcVel, otherSolid.ProcessingData.CalcVel, out float first)) {
					earliest = Math.Min(earliest, first);
				}

			}

			solid.ProcessingData.TimeOfImpact = earliest;

		}

		private static void MoveSolid(SolidRigidbody solid) {

			foreach (var actor in AllActors) {

				if (PhysicsMath.IntersectPush(solid.MainCollider, actor.MainCollider, solid.Velocity, solid.ProcessingData.TimeOfImpact, out Vector2 delta)) {

					//Console.WriteLine($"[1] delta push of {delta.ToStringHighPrecision()} to {(actor.Entity.Position + delta).ToStringHighPrecision()}");
					actor.Entity.Position += delta;

				}

			}

			solid.Entity.Position += solid.Velocity;

		}

		/// <summary>
		/// Collides the given actor against all other rigidbodies and calculate the allowed velocity during this frame.
		/// </summary>
		private static void CalculateVelocity(ActorRigidbody actor) {

			actor.ProcessingData.CalcVel = actor.Velocity;

			foreach (var rigidbody in AllRigidbodies) {

				// Skip self
				if (ReferenceEquals(rigidbody, actor)) continue;

				// Don't check same pair multiple times
				if (rigidbody.ProcessingData.CheckedAgainst.Contains(actor)) {
					Console.WriteLine($"Skipping {actor} v {rigidbody}");
					continue;
				}

				if (PhysicsMath.IsOverlapping(actor.MainCollider, rigidbody.MainCollider)) {

					// Prevent ghost vertices
					if (!PhysicsMath.IsSlidingCorner(actor.ProcessingData.CalcVel, actor.MainCollider, rigidbody.MainCollider)) {

						VelocityPair result = Ruleset.Get(actor, rigidbody);

						// Get relative velocity if other rigidbody was standing still
						Vector2 relativeVel = result.left - result.right;

						// Collide relative velocity against rigidbody surface
						Vector2 relativeAllowedVel = PhysicsMath.GetAllowedVelocity(relativeVel, PhysicsMath.GetNormal(actor.MainCollider, rigidbody.MainCollider));

						Console.WriteLine($"[{actor}] before {actor.ProcessingData.CalcVel}, after {relativeAllowedVel + result.right}");
						Console.WriteLine($"[{rigidbody}] before {rigidbody.ProcessingData.CalcVel}, after {result.right}");

						// Convert from relative velocity back to absolute velocity
						actor.ProcessingData.CalcVel = relativeAllowedVel + result.right;

						rigidbody.ProcessingData.CalcVel = result.right;

						// Blacklist the other rigidbody to prevent repeated checks
						rigidbody.ProcessingData.CheckedAgainst.Add(actor);
						actor.ProcessingData.CheckedAgainst.Add(rigidbody);

					}

				}

			}
		}

		/// <summary>
		/// Run dynamic intersection test against all colliders. Return the earliest time of impact, or <c>1.0</c> if no collisions occured.
		/// </summary>
		private static float GetEarliestTimeOfImpact(ActorRigidbody actor) {

			float time = 1f;

			//foreach (var otherActor in AllActors) {

			//	// Skip self
			//	if (ReferenceEquals(otherActor, actor)) continue;

			//	if (PhysicsMath.IntersectMoving(otherActor.MainCollider, actor.MainCollider, otherActor.ProcessingData.CalcVel, actor.ProcessingData.CalcVel, out float first)) {

			//		Console.WriteLine("other actor");
			//		time = Math.Min(time, first);

			//	}
			//}

			foreach (var solid in AllSolids) {

				if (PhysicsMath.IsInMovementPath(actor.MainCollider, solid.MainCollider, actor.ProcessingData.CalcVel, solid.Velocity)) {

					// The solid will overlap the actor during its movement. Use zero velocity for now and calculate delta push later.

					if (PhysicsMath.IntersectMoving(solid.MainCollider, actor.MainCollider, Vector2.Zero, actor.ProcessingData.CalcVel, out float first)) {

						Console.WriteLine("1");
						time = Math.Min(time, first);

					}

				} else {

					// The solid is moving away from actor. Include solid velocity in calculation.

					if (PhysicsMath.IntersectMoving(solid.MainCollider, actor.MainCollider, solid.Velocity, actor.ProcessingData.CalcVel, out float first)) {

						Console.WriteLine("2");
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

					if (PhysicsMath.IsInsideExact(actor.MainCollider, collider)) {

						var newPos = PhysicsMath.Depenetrate(actor.MainCollider, collider);
						//Console.WriteLine("Corrected by {0}", (actor.Entity.Position - newPos).ToStringHighPrecision());
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

		/// <summary>
		/// Returns true if the rigidbody velocity or the allowed velocity is equal to 0.
		/// </summary>
		private static bool IsStationary(Rigidbody rigidbody) {
			return rigidbody.Velocity.LengthSquared() == 0f || rigidbody.ProcessingData.CalcVel.LengthSquared() == 0f;
		}

	}

}