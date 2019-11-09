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
		public bool Calculated;
		public float TimeOfImpact;

		public void Reset() {
			Calculated = false;
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
		public static bool DebugDrawColliders = false;

		// Physic Object Lists
		public static readonly List<BoxCollider> AllColliders = new List<BoxCollider>();
		public static readonly List<ActorRigidbody> AllActors = new List<ActorRigidbody>();
		public static readonly List<SolidRigidbody> AllSolids = new List<SolidRigidbody>();
		public static readonly List<StaticRigidbody> AllStatics = new List<StaticRigidbody>();
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
			Console.WriteLine("=======");
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
				CalculateSolidVelocity(solid);
			}

			// Calculate how far the solid can move this update.
			foreach (var solid in AllSolids) {
				if (solid.ProcessingData.CalcVel == Vector2.Zero) continue;
				CalculateSolidEarliestTimeOfImpact(solid);
			}

			// Move the solid, drag attached actors, push actors in movement path, crush sandwiched actors.
			foreach (var solid in AllSolids) {
				if (solid.ProcessingData.CalcVel == Vector2.Zero) continue;
				MoveSolid(solid);
			}

			// Get allowed velocity for all actors.
			foreach (var actor in AllActors) {
				CalculateActorVelocity(actor);
			}

			// Calculate where to move actors
			foreach (var actor in AllActors) {
				if (actor.ProcessingData.CalcVel == Vector2.Zero) continue;

				CalculateActorEarliestTimeOfImpact(actor);

				actor.ProcessingData.MoveDelta = actor.ProcessingData.CalcVel * actor.ProcessingData.TimeOfImpact;

			}

			// Move actors to calculated position
			foreach (var actor in AllActors) {
				if (actor.ProcessingData.CalcVel == Vector2.Zero) continue;

				actor.Entity.Position += actor.ProcessingData.MoveDelta;
			}

		}

		#region Solids Narrowphase

		private static void CalculateSolidVelocity(SolidRigidbody solid) {

			solid.ProcessingData.CalcVel = solid.Velocity;

			if (solid.Velocity == Vector2.Zero) return; // Early exit if no velocity.

			// Regional check against all static rigidbodies
			foreach (var staticbody in AllStatics) {

				foreach (var collider in solid.Colliders) {

					if (solid.ProcessingData.CalcVel == Vector2.Zero) return;

					if (PhysicsMath.IsOverlapping(collider, staticbody.MainCollider)) {

						if (!PhysicsMath.IsSlidingCorner(collider, staticbody.MainCollider)) {

							solid.ProcessingData.CalcVel = VelocityMath.IntoPlane(solid.ProcessingData.CalcVel, PhysicsMath.GetNormal(collider, staticbody.MainCollider));

							Console.WriteLine($"Solid collision against {staticbody.Entity} ({collider.LocalPosition}) to yield {solid.ProcessingData.CalcVel}");

						}

					}

				}

			}

			// Regional check against other solids
			foreach (var otherSolid in AllSolids) {

				if (solid.ProcessingData.CalcVel == Vector2.Zero) return;

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				// Skip if other solid has already processed this solid
				if (otherSolid.ProcessingData.CheckedAgainst.Contains(solid)) continue;


				if (PhysicsMath.IsOverlapping(solid.MainCollider, otherSolid.MainCollider)) {

					if (!PhysicsMath.IsSlidingCorner(solid.MainCollider, otherSolid.MainCollider)) {

						VelocityPair result = Ruleset.Get(solid, otherSolid);

						Console.WriteLine($"Collided {solid.Entity} against {otherSolid.Entity} to yield {result.left} and {result.right}");

						solid.ProcessingData.CalcVel = result.left;
						otherSolid.ProcessingData.CalcVel = result.right;

						solid.ProcessingData.CheckedAgainst.Add(otherSolid);
						otherSolid.ProcessingData.CheckedAgainst.Add(solid);

					}

				}

			}

		}

		private static void CalculateSolidEarliestTimeOfImpact(SolidRigidbody solid) {

			Console.WriteLine("running on " + solid.Entity);

			float earliest = 1f;

			// Regional check against all statics
			foreach (var staticbody in AllStatics) {

				foreach (var collider in solid.Colliders) {

					if (earliest == 0f) break;

					if (PhysicsMath.IntersectMoving(collider, staticbody.MainCollider, solid.ProcessingData.CalcVel, Vector2.Zero, out float first)) {

						Console.WriteLine($"{solid.Entity} ({collider.LocalPosition}) hit {staticbody.Entity} at time {first}");
						earliest = Math.Min(earliest, first);

					}

				}

			}

			// Regional check against all solids
			foreach (var otherSolid in AllSolids) {

				if (earliest == 0f) break;

				// Skip self
				if (ReferenceEquals(solid, otherSolid)) continue;

				if (PhysicsMath.IntersectMoving(solid.MainCollider, otherSolid.MainCollider, solid.ProcessingData.CalcVel, otherSolid.ProcessingData.CalcVel, out float first)) {
					Console.WriteLine($"{solid.Entity} hit {otherSolid.Entity} at time {first}");
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

		#endregion

		#region Actor Narrowphase

		/// <summary>
		/// Collides the given actor against all other rigidbodies and calculate the allowed velocity during this frame.
		/// </summary>
		/// <remarks>This function will immediately return if the calculated velocity becomes zero.</remarks>
		private static void CalculateActorVelocity(ActorRigidbody actor) {

			actor.ProcessingData.CalcVel = actor.Velocity;

			if (actor.Velocity == Vector2.Zero) return;


			foreach (var staticbody in AllStatics) {

				if (actor.ProcessingData.CalcVel == Vector2.Zero) return;

				if (PhysicsMath.IsOverlapping(actor.MainCollider, staticbody.MainCollider)) {

					if (!PhysicsMath.IsSlidingCorner(actor.MainCollider, staticbody.MainCollider)) {

						actor.ProcessingData.CalcVel = VelocityMath.IntoPlane(actor.ProcessingData.CalcVel, PhysicsMath.GetNormal(actor.MainCollider, staticbody.MainCollider));

					}

				}

			}

			foreach (var solid in AllSolids) {

				foreach (var solidCollider in solid.Colliders) {

					if (actor.ProcessingData.CalcVel == Vector2.Zero) return;

					if (PhysicsMath.IsOverlapping(actor.MainCollider, solidCollider)) {

						if (!PhysicsMath.IsSlidingCorner(actor.MainCollider, solidCollider)) {

							actor.ProcessingData.CalcVel = VelocityMath.IntoPlane(actor.ProcessingData.CalcVel, PhysicsMath.GetNormal(actor.MainCollider, solidCollider));

						}

					}

				}

			}

		}

		/// <summary>
		/// Run dynamic intersection test against all colliders. Return the earliest time of impact, or <c>1.0f</c> if no collisions occured.
		/// </summary>
		/// <remarks>This function will finish immediately if the earliest time of impact reaches 0.</remarks>
		private static void CalculateActorEarliestTimeOfImpact(ActorRigidbody actor) {

			float time = 1f;

			foreach (var staticbody in AllStatics) {

				if (time == 0) break;

				if (PhysicsMath.IntersectMoving(staticbody.MainCollider, actor.MainCollider, Vector2.Zero, actor.ProcessingData.CalcVel, out float first)) {

					time = Math.Min(time, first);

				}

			}

			foreach (var solid in AllSolids) {

				foreach (var solidCollider in solid.Colliders) {

					if (time == 0) break;

					if (PhysicsMath.IntersectMoving(solidCollider, actor.MainCollider, Vector2.Zero, actor.ProcessingData.CalcVel, out float first)) {

						time = Math.Min(time, first);

					}

				}

			}

			actor.ProcessingData.TimeOfImpact = time;
		}

		#endregion

		#endregion // End Narrowphase

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

			foreach (var rb in AllRigidbodies) {
				// draw velocity
				MDraw.DrawLine(rb.Entity.Position, rb.Entity.Position + rb.Velocity / Time.DeltaTime, Color.Gray);
				MDraw.DrawPoint(rb.Entity.Position + rb.Velocity / Time.DeltaTime, Color.Gray);
			}

			MDraw.End();
		}

	}

}