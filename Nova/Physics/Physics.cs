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

		public static BoxCollider ground, player;
		public static Actor actor;

		static Vector2 hitpointFirst, allowedVelocity;

		public static void Update() {

			allowedVelocity = actor.Velocity;

			if (PhysicsMath.IntersectMovingBoxAgainstBoxNoOverlap(ground, player, Vector2.Zero, allowedVelocity, out float first)) {
				//Console.WriteLine("HIT");
				hitpointFirst = actor.Entity.Position + allowedVelocity * first;
				actor.Entity.Position += allowedVelocity * first;
			} else {
				//Console.WriteLine("MISS");
				hitpointFirst = 1000 * Vector2.One;
				actor.Entity.Position += allowedVelocity;
			}

			if (PhysicsMath.OverlapBoxAgainstBox(ground, player)) {
				actor.Entity.Position = PhysicsMath.DepenetrateBoxAgainstBox(player, ground);
				Console.WriteLine("depen");
			}

			var delta = ground.Entity.Position - actor.Entity.Position;
			Console.WriteLine($"Actor {actor.Entity.Position}, Solid {ground.Entity.Position}, Delta {delta}, Delta Mag {delta.Length()}");

		}

		public static void Draw() {
			MDraw.Begin();
			MDraw.DrawPoint(hitpointFirst, Color.Red);
			MDraw.DrawPoint(actor.Entity.Position + allowedVelocity, Color.White);
			MDraw.End();
		}

		static void Old1() {
			foreach (var actor in AllActors) {

				// TODO: add gravity

				Vector2 allowedVelocity = actor.Velocity;
				//foreach (var c in actor.Contacts) {
				//	allowedVelocity = PhysicsMath.GetAllowedVelocity(allowedVelocity, c.normal);
				//	Console.WriteLine(c.normal);
				//}

				//Console.WriteLine("Desired: {0}, Allowed {1}", actor.Velocity, allowedVelocity);

				var newHits = new List<Tuple<float, Collider>>();

				foreach (var toCollide in AllColliders.Cast<BoxCollider>()) {

					// ignore colliders on current actor
					if (actor.Colliders.Contains(toCollide)) continue;

					// ignore colliders that are already being touched
					//if (ContactListContainsCollider(actor.Contacts, toCollide)) continue;

					Console.WriteLine("comparing with {0}", toCollide);

					if (PhysicsMath.IntersectMovingBoxAgainstBoxNoOverlap((BoxCollider)actor.Colliders[0], toCollide, allowedVelocity, Vector2.Zero, out float first)) {
						Console.WriteLine("hit at {0} moving {1}", first, allowedVelocity);
						newHits.Add(new Tuple<float, Collider>(first, toCollide));

					}

				}

				//Console.WriteLine($"{actor.Velocity}, t={time}, {actor.Entity.Position + time * actor.Velocity}");

				float moveTime = 1f;

				var newContactList = new List<ContactPoint>();

				foreach (var item in newHits) {
					moveTime = Math.Min(item.Item1, moveTime);
					Console.WriteLine("hit of time " + item.Item1);

					//var projectedPosition = actor.Entity.Position + first * allowedVelocity;
					//var normal = PhysicsMath.GetClosestNormalOnBox(projectedPosition, toCollide);
				}

				actor.Entity.Position += allowedVelocity * moveTime;

				return;

				var toRecheck = newHits.Select(x => x.Item2).Concat(actor.Contacts.Select(x => x.collider));

				foreach (var item in toRecheck) {
					if (PhysicsMath.OverlapBoxAgainstBox((BoxCollider)item, (BoxCollider)actor.Colliders[0])) {
						//Console.WriteLine("blacklist overlap");
						newContactList.Add(new ContactPoint() {
							collider = item,
							normal = PhysicsMath.ClosestPointOnBox(actor.Colliders[0].Position, (BoxCollider)item)
						});
					}
				}

				if (actor.Colliders.Count == 0 && newContactList.Count > 0) {
					Console.WriteLine("First hit");
				}
				actor.SetContacts(newContactList);

			}
		}

		static bool ContactListContainsCollider(IEnumerable<ContactPoint> points, Collider c) {
			foreach (var item in points) {
				if (item.collider == c) {
					return true;
				}
			}
			return false;
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