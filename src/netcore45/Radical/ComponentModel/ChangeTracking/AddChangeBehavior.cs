namespace Topics.Radical.ComponentModel.ChangeTracking
{

	/// <summary>
	/// Tha AddChangeBehavior enumeration describes the reason
	/// of the change add request.
	/// </summary>
	public enum AddChangeBehavior
	{
		/// <summary>
		/// None is a default not supported value.
		/// </summary>
		None = 0,

		/// <summary>
		/// The change comes from the external environment, 
		/// eg. is pushed because of user action.
		/// </summary>
		Default,

		/// <summary>
		/// The change is due to a Redo request.
		/// </summary>
		RedoRequest,

		/// <summary>
		/// The change is due to an Undo request.
		/// </summary>
		UndoRequest
	}
}
