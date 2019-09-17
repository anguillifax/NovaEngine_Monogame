namespace Nova {

	public abstract class BaseComponent {

		public Scene Scene;
		public BaseEntity Entity;

		public BaseComponent(BaseEntity parent) {
			Entity = parent;
			Scene = parent.Scene;
		}

		public virtual void PreUpdate() {
		}

		public virtual void Update() {
		}

		public virtual void PostUpdate() {
		}

	}

	public abstract class Component {

		public Scene Scene;
		public Entity Entity;

		public Component(Entity parent) {
			Entity = parent;
			Scene = parent.Scene;
		}

		public virtual void PreUpdate() {
		}

		public virtual void Update() {
		}

		public virtual void PostUpdate() {
		}

	}

}