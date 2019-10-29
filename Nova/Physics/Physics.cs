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
		public Vector2 MoveDelta;
		public Vector2 CalcVel;
		public List<Rigidbody> CheckedAgainst; // TODO: optimize with a HashSet
		public float TimeOfImpact;
		public Vector2 Normal;

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
			//ResolvingPhase();
			// GenerateContacts();

			Console.WriteLine();
			Console.WriteLine();

		}

		private static void PreConfigure() {

			if (Ruleset == null) {
				Ruleset = new DefaultVelocityCombinationRuleset();
			}

			// TODO: Optimize by storing rigidbodies that need reset.
			foreach (var rigidbody in AllRigidbodies) {
				rigidbody.ProcessingData.Reset();
			}
		}

		#region Narrowphase

		private static void Narrowphase() {

			// Get allowed velocity for all solids.
			foreach (var solid in AllSolids) {
				if (solid.Velocity.LengthSquared() == 0f || solid.Stationary) {
					solid.ProcessingData.CalcVel = Vector2.Zero;
				} else {
					CalculateVelocity(solid);
				}
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

				CalculateEarliestTimeOfImpact(actor);

				actor.ProcessingData.MoveDelta = actor.ProcessingData.CalcVel * actor.ProcessingData.TimeOfImpact;

			}

			// Move actors to calculated position
			foreach (var actor in AllActors) {
				if (IsStationary(actor)) continue;

				actor.Entity.Position += actor.ProcessingData.MoveDelta;
			}

		}

		private static void CalculateVelocity(SolidRigidbody solid) {

			solid.ProcessingData.CalcVel = solid.Velocity;

			foreach (var otherSolid in AllSolids) {

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				// Skip if other solid has already processed this solid
				if (otherSolid.ProcessingData.CheckedAgainst.Contains(solid)) continue;

				if (PhysicsMath.IsOverlapping(solid.MainCollider, otherSolid.MainCollider)) {

					// Check for ghost vertices
					if (!PhysicsMath.IsSlidingCorner(solid.MainCollider, otherSolid.MainCollider)) {

						if (otherSolid.Stationary) {
							solid.ProcessingData.CalcVel = PhysicsMath.GetAllowedVelocity(solid.ProcessingData.CalcVel,
								PhysicsMath.GetNormal(solid.MainCollider, otherSolid.MainCollider));

						} else {

							VelocityPair result = Ruleset.Get(solid, otherSolid);

							Vector2 relative = solid.ProcessingData.CalcVel - result.right;
							Vector2 relativeAllowed = PhysicsMath.GetAllowedVelocity(relative, PhysicsMath.GetNormal(solid.MainCollider, otherSolid.MainCollider));
							solid.ProcessingData.CalcVel = relativeAllowed + result.right;

							Vector2 otherRelative = otherSolid.ProcessingData.CalcVel - result.left;
							Vector2 otherRelativeAllowed = PhysicsMath.GetAllowedVelocity(otherRelative, PhysicsMath.GetNormal(otherSolid.MainCollider, solid.MainCollider));
							otherSolid.ProcessingData.CalcVel = otherRelativeAllowed + result.left;

							solid.ProcessingData.CheckedAgainst.Add(otherSolid);
							otherSolid.ProcessingData.CheckedAgainst.Add(solid);

						}

					}

				}

			}

		}

		private static void CalculateEarliestTimeOfImpact(SolidRigidbody solid) {

			float earliest = 1f;

			foreach (var otherSolid in AllSolids) {

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				if (PhysicsMath.IntersectMoving(solid.MainCollider, otherSolid.MainCollider, solid.ProcessingData.CalcVel, otherSolid.ProcessingData.CalcVel, out float first)) {
					earliest = Math.Min(earliest, first);
				}

			}

			solid.ProcessingData.TimeOfImpact = earliest;

		}

		private static void MoveSolid(SolidRigidbody solid) {

			List<ActorRigidbody> pushList = new List<ActorRigidbody>();
			List<ActorRigidbody> relocateList = new List<ActorRigidbody>();

			// Run global check to see if any actors are attached.
			foreach (var actor in AllActors) {

				if (actor.IsAttached(solid)) {
					pushList.Add(actor);
				}

			}

			// Run regional check to find any actors that need to be moved during this movement update.
			foreach (var actor in AllActors) {

				// Skip if actor has already indicated it should be moved.
				if (pushList.Contains(actor)) continue;

				if (PhysicsMath.IntersectPush(solid.MainCollider, actor.MainCollider, solid.Velocity, solid.ProcessingData.TimeOfImpact, out Vector2 delta)) {

					pushList.Add(actor);
					actor.ProcessingData.MoveDelta = delta;

				} else if (PhysicsMath.ShouldDrag(actor.MainCollider, solid.MainCollider, actor.Velocity, solid.ProcessingData.CalcVel)) {

					relocateList.Add(actor);

				}

			}

			foreach (var actor in pushList) {

				actor.Entity.Position += actor.ProcessingData.MoveDelta;

				foreach (var otherSolid in AllSolids) {

					// Skip solid that actor was pushed by
					if (ReferenceEquals(solid, otherSolid)) continue;

					// If resultant position is inside another solid, then actor has to be crushed.
					if (PhysicsMath.IsInside(actor.MainCollider, otherSolid.MainCollider)) {
						Console.WriteLine($"Crush {actor} against {otherSolid}");
						actor.Crush();
					}

				}

			}

			Vector2 solidDelta = solid.ProcessingData.CalcVel * solid.ProcessingData.TimeOfImpact;
			solid.Entity.Position += solidDelta;

			foreach (var actor in relocateList) {

				actor.Entity.Position += solidDelta;
				Console.WriteLine($"relocated {actor} by {solidDelta}");

			}

		}

		/// <summary>
		/// Collides the given actor against all other rigidbodies and calculate the allowed velocity during this frame.
		/// </summary>
		private static void CalculateVelocity(ActorRigidbody actor) {

			actor.ProcessingData.CalcVel = actor.Velocity;

			// TODO: Collide against other actors
			foreach (var otherActor in AllActors) {

				// Skip self
				if (ReferenceEquals(actor, otherActor)) continue;

				// Don't check same pair multiple times
				if (otherActor.ProcessingData.CheckedAgainst.Contains(actor)) {
					Console.WriteLine($"Skipping {actor} v {otherActor}");
					continue;
				}

			}


			foreach (var solid in AllSolids) {

				if (PhysicsMath.IsOverlapping(actor.MainCollider, solid.MainCollider)) {

					// Prevent ghost vertices
					if (!PhysicsMath.IsSlidingCorner(actor.MainCollider, solid.MainCollider)) {

						var va = PhysicsMath.GetAllowedVelocity(actor.ProcessingData.CalcVel, PhysicsMath.GetNormal(actor.MainCollider, solid.MainCollider));

						//Console.WriteLine($"[{actor}] before {actor.ProcessingData.CalcVel}, after {va}");

						actor.ProcessingData.CalcVel = va;

					}

				}

			}

		}

		/// <summary>
		/// Run dynamic intersection test against all colliders. Return the earliest time of impact, or <c>1.0f</c> if no collisions occured.
		/// </summary>
		private static void CalculateEarliestTimeOfImpact(ActorRigidbody actor) {

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

				if (PhysicsMath.IntersectMoving(solid.MainCollider, actor.MainCollider, Vector2.Zero, actor.ProcessingData.CalcVel, out float first)) {

					time = Math.Min(time, first);

				}

			}

			actor.ProcessingData.TimeOfImpact = time;
		}

		#endregion


		#region Resolving Phase

		private static void ResolvingPhase() {

			foreach (var actor in AllActors) {

				foreach (var collider in AllColliders) {
					if (actor.Colliders.Contains(collider)) continue; // skip self

					if (PhysicsMath.IsInsideExact(actor.MainCollider, collider)) {

						var newPos = PhysicsMath.Depenetrate(actor.MainCollider, collider);
						Console.WriteLine("Corrected by {0}", (actor.Entity.Position - newPos).ToStringFixed(8));
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
		/// <para>Do not use when calculating allowed velocity. CalcVel needs to be recalculated first.</para>
		/// </summary>
		private static bool IsStationary(Rigidbody rigidbody) {
			return rigidbody.Stationary || rigidbody.ProcessingData.CalcVel.LengthSquared() == 0f;
		}

	}

}