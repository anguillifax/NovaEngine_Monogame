namespace Nova {

	public abstract class Component {

		public Scene Scene {
			get {
				if (Entity != null) {
					return Entity.Scene;
				} else {
					return null;
				}
			}
		}
		public Entity Entity { get; protected set; }
		public bool Active { get; set; }

		public Component(Entity parent) {
			Entity = parent;
			Entity?.AddComponent(this);
			Active = true;
		}

		public virtual void Detach() {
			Entity?.RemoveComponent(this);
			Entity = null;
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