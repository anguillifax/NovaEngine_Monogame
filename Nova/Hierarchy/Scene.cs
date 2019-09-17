using System;
using System.Collections.Generic;

namespace Nova {

	public class Scene {

		public string Name { get; private set; }

		public readonly List<BaseEntity> entities;
		private readonly List<BaseEntity> toAdd;
		private readonly List<BaseEntity> toRemove;

		public Scene(string name) {
			Name = name;
			entities = new List<BaseEntity>();
			toAdd = new List<BaseEntity>();
			toRemove = new List<BaseEntity>();
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
			foreach (var item in toAdd) {
				entities.Add(item);
			}
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
			// Test if object is a drawable entity. Then draw if visible.
			foreach (var item in entities) {
				if (item.Active && item is Entity i) {
					if (i.Visible) i.Draw();
				}
			}
		}

		public void Add(BaseEntity entity) {
			if (entity != null) {
				toAdd.Add(entity);
				entity.Scene = this;
			}
		}

		public void Remove(BaseEntity entity) {
			if (entity != null) {
				toRemove.Add(entity);
				entity.Scene = null;
			}
		}

		/// <summary>
		/// Moves an entity from one scene to another
		/// </summary>
		public static void MoveEntity(Scene from, Scene to, BaseEntity entity) {
			if (from == null || to == null || entity == null) throw new ArgumentNullException();
			from.Remove(entity);
			to.Add(entity);
		}

	}

}