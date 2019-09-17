using System.Collections.Generic;

namespace Nova {

	public abstract class BaseEntity {

		public Scene Scene { get; set; }
		public readonly List<Component> Components;

		protected BaseEntity(Scene scene, bool active = true) {
			Active = active;
			Scene = scene;
			Components = new List<Component>();
		}

		public bool Active { get; set; }

		public virtual void PreUpdate() {
			foreach (var item in Components) {
				item.PreUpdate();
			}
		}

		public virtual void Update() {
			foreach (var item in Components) {
				item.Update();
			}
		}

		public virtual void PostUpdate() {
			foreach (var item in Components) {
				item.PostUpdate();
			}
		}

	}

}