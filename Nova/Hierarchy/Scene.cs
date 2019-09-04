using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nova {

	public class Scene {

		public readonly List<Entity> entities;
		private readonly List<Entity> toAdd;
		private readonly List<Entity> toRemove;

		public Scene() {
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
				if (item is EntityDrawable i) {
					if (i.Visible) i.Draw();
				}
			}
		}

		public void Add(Entity entity) {
			if (entity != null) {
				toAdd.Add(entity);
			}
		}

		public void Remove(Entity entity) {
			if (entity != null) {
				toRemove.Add(entity);
			}
		}

	}

}