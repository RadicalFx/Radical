namespace Topics.Radical.ComponentModel.Factories
{
	/// <summary>
	/// The main entry point for generic data access and 
	/// conversation management.
	/// </summary>
	[Contract]
	public interface IDataContextFactory
	{
		/// <summary>
		/// Creates a new data context, a data context identifies a conversation.
		/// </summary>
		/// <returns>The newly created data context.</returns>
		IDataContext Create();

		///// <summary>
		///// Creates a new stateless data context, a data context identifies a conversation.
		///// </summary>
		///// <returns>The newly created data context.</returns>
		//IDataContext CreateStateless();
	}
}
