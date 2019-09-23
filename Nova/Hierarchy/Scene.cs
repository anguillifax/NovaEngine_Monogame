using System;
using System.Collections.Generic;

namespace Nova {

	public class Scene {

		public string Name { get; private set; }

		public readonly List<Entity> entities;
		private readonly List<Entity> toAdd;
		private readonly List<Entity> toRemove;

		public Scene(string name) {
			Name = name;
			entities = new List<Entity>();
			toAdd = new List<Entity>();
			toRemove = new List<Entity>();
		}

		public void PreUpdate() {
			// Call PreUpdate() on active entities
			foreach (var item in entities) {
				if (item.Active) item.PreUpdate();
			}
		}

		public void Update() {
			// Call Update() on active entities
			foreach (var item in entities) {
				if (item.Active) item.Update();
			}
		}

		public void PostUpdate() {
			// Add entities to scene list
			entities.AddRange(toAdd);
			toAdd.Clear();
			// Remove entities from scene list
			foreach (var item in toRemove) {
				entities.Remove(item);
			}
			toRemove.Clear();

			// Call PostUpdate() on active entities
			foreach (var item in entities) {
				if (item.Active) item.PostUpdate();
			}
		}

		public void Draw() {
			// Draw active and visible entities.
			foreach (var item in entities) {
				if (item.Active && item.Visible) {
					item.Draw();
				}
			}
		}

		public void Add(Entity entity) {
			if (entity != null) {
				toAdd.Add(entity);
				entity.Scene = this;
			}
		}

		public void Remove(Entity entity) {
			if (entity != null) {
				toRemove.Add(entity);
				entity.Scene = null;
			}
		}

		/// <summary>
		/// Move an entity from one scene to another
		/// </summary>
		public static void MoveEntity(Scene from, Scene to, Entity entity) {
			if (from == null || to == null || entity == null) throw new ArgumentNullException();
			from.Remove(entity);
			to.Add(entity);
		}

	}

}