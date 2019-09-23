using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Nova {

	public abstract class Entity {

		public bool Active { get; set; }
		public bool Visible { get; set; }

		public Scene Scene { get; set; }
		public readonly List<Component> ComponentList;
		private readonly List<Component> toAdd;
		private readonly List<Component> toRemove;

		private IntVector2 m_Position;
		public IntVector2 Position { get => m_Position; set => m_Position = value; }
		private Vector2 m_Scale;
		public Vector2 Scale { get => m_Scale; set => m_Scale = value; }
		public float Rotation { get; set; }

		#region Transform Component Properties

		public int PosX {
			get => Position.X;
			set => m_Position.X = value;
		}
		public int PosY {
			get => Position.Y;
			set => m_Position.Y = value;
		}

		public float ScaleX {
			get => Scale.X;
			set => m_Scale.X = value;
		}
		public float ScaleY {
			get => Scale.Y;
			set => m_Scale.Y = value;
		}

		#endregion

		protected Entity(Scene scene, IntVector2 position, bool startActive = true, bool startVisible = true) {
			Active = startActive;
			Visible = startVisible;
			Scene = scene;
			Position = position;
			Scale = Vector2.One;
			Rotation = 0f;
			ComponentList = new List<Component>();
			toAdd = new List<Component>();
			toRemove = new List<Component>();
		}

		public void AddComponent<T>(T c) where T : Component {
			toAdd.Add(c);
		}

		public void RemoveComponent<T>(T c) where T : Component {
			toRemove.Add(c);
		}

		public virtual void PreUpdate() {
			foreach (var item in ComponentList) {
				item.PreUpdate();
			}
		}

		public virtual void Update() {
			foreach (var item in ComponentList) {
				item.Update();
			}
		}

		public virtual void PostUpdate() {
			// Add entities to scene list
			ComponentList.AddRange(toAdd);
			toAdd.Clear();
			// Remove entities from scene list
			foreach (var item in toRemove) {
				ComponentList.Remove(item);
			}
			toRemove.Clear();

			foreach (var item in ComponentList) {
				item.PostUpdate();
			}
		}

		public virtual void Draw() {
			foreach (var item in ComponentList) {
				item.Draw();
			}
		}

	}

}