using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

		protected Entity(Scene scene, Vector2 position, bool startActive = true, bool startVisible = true) {
			Active = startActive;
			Visible = startVisible;
			Scene = scene;
			Position = position;
			Scale = Vector2.One;
			Rotation = 0f;

			ComponentList = new List<Component>();
		}

		public void AddComponent<T>(T c) where T : Component {
			ComponentList.Add(c);
		}

		public void RemoveComponent<T>(T c) where T : Component {
			ComponentList.Add(c);
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