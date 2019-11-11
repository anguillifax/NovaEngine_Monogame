namespace Nova.Input {

	/// <summary>
	/// Defines all available controls in the game.
	/// </summary>
	public abstract class GameControls {

		public VirtualButton Enter;
		public VirtualButton Back;
		public VirtualButton Clear;

		public VirtualButton Jump;
		public VirtualButton Attack;
		public VirtualButton Unleash;
		public VirtualButton Retry;

		public VirtualAxis Horizontal;
		public VirtualAxis Vertical;

	}

	public static class BindingNames {
		public static readonly string Enter = "enter";
		public static readonly string Back = "back";
		public static readonly string Clear = "clear";

		public static readonly string Horz = "horz";
		public static readonly string Vert = "vert";

		public static readonly string Jump = "jump";
		public static readonly string Attack = "attack";
		public static readonly string Unleash = "unleash";
		public static readonly string Retry = "retry";
	}

}