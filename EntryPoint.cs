using System;

namespace Nova {
#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class EntryPoint {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main() {
			using (var game = new Engine()) {
				game.Run();
			}
		}
	}
#endif
}
