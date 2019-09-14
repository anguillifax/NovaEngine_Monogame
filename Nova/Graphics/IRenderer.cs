namespace Nova {

	public interface IRenderer {

		VisualEntity Entity { get; set; }

		void Render();

	}

}