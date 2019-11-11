namespace Nova.Input {

	public abstract class VirtualAxis {

		public string Name { get; private set; }

		public float Value { get; protected set; }

		public readonly InputRepeater RepeaterPos;
		public readonly InputRepeater RepeaterNeg;

		protected VirtualAxis(string name) {
			Name = name;
			RepeaterPos = new InputRepeater(() => Value > GlobalInputProperties.AxisDeadzone);
			RepeaterNeg = new InputRepeater(() => Value < -GlobalInputProperties.AxisDeadzone);
			InputManager.InputUpdate += Update;
			InputManager.InputUpdate += RepeaterPos.Update;
			InputManager.InputUpdate += RepeaterNeg.Update;
		}

		protected abstract void Update();

		public override string ToString() {
			return string.Format("VirtualAxis \"{0}\" : {1}", Name, Value);
		}

	}

}