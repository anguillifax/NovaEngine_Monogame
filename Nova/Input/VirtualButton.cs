using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Nova.Input {

	public enum RebindResult {
		Added, Removed, NoOp
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