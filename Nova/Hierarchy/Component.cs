namespace Nova {

	public abstract class Component {

		public Scene Scene { get; protected set; }
		public Entity Entity { get; protected set; }

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

		public virtual void Draw() {
		}

	}

}