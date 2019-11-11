namespace Nova.Input {

	public enum RebindResult {
		Added, Removed, NoOp, NotAllowed
	}

	public abstract class VirtualButton {

		public string Name { get; protected set; }

		public abstract bool Pressed { get; }
		public abstract bool JustPressed { get; }
		public abstract bool Released { get; }
		public abstract bool JustReleased { get; }

		protected VirtualButton(string name) {
			Name = name;
		}

		public override string ToString() {
			return string.Format("VirtualButton \"{0}\" ({1} {2} {3} {4})", Name, 
				Pressed ? "P" : "", JustPressed ? "JP" : "", Released ? "R" : "", JustReleased ? "JP" : "");
		}

	}

	/// <summary>
	/// Provides an Update() method and implements { Pressed, JustPressed, Released, JustReleased } using { value, lastValue }
	/// </summary>
	public abstract class VirtualButtonBaseLogic : VirtualButton {

		protected bool value, lastValue;

		public VirtualButtonBaseLogic(string name) :
			base(name) {
			InputManager.InputUpdate += Update;
			BindingManager.SaveBindings += OnSaveBinding;
			BindingManager.LoadBindings += OnLoadBinding;
		}

		protected abstract void Update();

		protected abstract void OnLoadBinding();
		protected abstract void OnSaveBinding();

		/// <summary>
		/// Returns true if button is pressed
		/// </summary>
		public override bool Pressed {
			get { return value; }
		}

		/// <summary>
		/// Returns true if button was pressed this frame
		/// </summary>
		public override bool JustPressed {
			get { return value && value != lastValue; }
		}

		/// <summary>
		/// Returns true if button not being pressed
		/// </summary>
		public override bool Released {
			get { return !value; }
		}

		/// <summary>
		/// Returns true if button was released this frame
		/// </summary>
		public override bool JustReleased {
			get { return !value && value != lastValue; }
		}

	}

}