namespace AuthorIntrusion
{
	/// <summary>
	/// Defines the signature for a class that creates and manages the GUI
	/// application lifecycle.
	/// </summary>
	public interface IGuiFactory
	{
		/// <summary>
		/// Gets a value indicating whether this instance is valid for the
		/// current environment.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </value>
		bool IsValid { get; }

		/// <summary>
		/// Gets the priority of the GUI factory. The higher the number, the
		/// more applicable the factory is for the specific environment and
		/// window toolkit.
		/// </summary>
		int Priority { get; }

		/// <summary>
		/// Starts the GUI interface and manage the application lifecycle.
		/// </summary>
		void Start();
	}
}