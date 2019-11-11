using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Nova {

	public abstract class Entity {

		public bool Active { get; set; }
		public bool Visible { get; set; }

		private Scene _scene;
		public Scene Scene {
			get {
				return _scene;
			}
			set {
				if (_scene != value) {
					_scene?.Remove(this);
					_scene = value;
					_scene?.Add(this);
				}
			}
		}

		public readonly List<Component> ComponentList;

		public Vector2 Position;
		public Vector2 Scale;
		public float Rotation { get; set; }

		protected Entity(Scene scene, Vector2 position) {
			Active = true;
			Visible = true;
			Scene = scene;
			Position = position;
			Scale = Vector2.One;
			Rotation = 0f;

			ComponentList = new List<Component>();
		}

		public void AddComponent<T>(T c) where T : Component {
			if (c != null) {
				ComponentList.Add(c);
			}
		}

		public void RemoveComponent<T>(T c) where T : Component {
			if (c != null) {
				ComponentList.Add(c);
			}
		}

		/// <summary>
		/// Returns the first component of type T. If not found, returns null.
		/// </summary>
		public T GetComponent<T>() where T : Component {
			foreach (var item in ComponentList) {
				if (item is T) {
					return (T)item;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns all components of type T. Returns an empty collection if none are found.
		/// </summary>
		public IEnumerable<T> GetComponents<T>() where T : Component {
			return ComponentList.Where(x => x is T).Cast<T>();
		}

		public virtual void Init() {
			foreach (var item in ComponentList) {
				item.Init();
			}
		}

		public virtual void PreUpdate() {
			foreach (var item in ComponentList) {
				if (item.Active) item.PreUpdate();
			}
		}

		public virtual void Update() {
			foreach (var item in ComponentList) {
				if (item.Active) item.Update();
			}
		}

		public virtual void PostUpdate() {
			foreach (var item in ComponentList) {
				if (item.Active) item.PostUpdate();
			}
		}

		public virtual void Draw() {
			foreach (var item in ComponentList) {
				if (item.Active) item.Draw();
			}
		}

	}

}