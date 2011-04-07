namespace AuthorIntrusion.Contracts.Processors
{
	/// <summary>
	/// Handles the common functionality of a processor that only allows a
	/// single configuration object to control its behavior.
	/// </summary>
	public abstract class SingletonProcessor : ProcessorInfo, IProcessor
	{
		#region Information

		/// <summary>
		/// Returns <see langword="true"/> to indicate that is a singleton processor.
		/// </summary>
		public bool IsSingleton
		{
			get { return true; }
		}

		/// <summary>
		/// Returns the info object for the singleton processor.
		/// </summary>
		/// <returns>The singleton processor object.</returns>
		public ProcessorInfo CreateProcessorInfo()
		{
			return this;
		}

		#endregion
	}
}