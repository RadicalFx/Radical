namespace Topics.Radical.ChangeTracking
{
	using System;
	using Topics.Radical;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Validation;

	/// <summary>
	/// Represents a suggested action produced by
	/// the provisioning system of a change tracking service.
	/// </summary>
	public class AdvisedAction : IAdvisedAction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AdvisedAction"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="action">The action.</param>
		public AdvisedAction( Object target, ProposedActions action )
		{
			Ensure.That( target ).Named( () => target ).IsNotNull();
			Ensure.That( action )
				.Named( () => action )
				.IsDefined()
				.WithMessage( "None is a not supported ProposedActions value." )
				.If( v => v == ProposedActions.None )
				.ThenThrow( v => new NotSupportedException( v.GetFullErrorMessage() ) );

			this.Target = target;
			this.Action = action;
		}

		#region IAdvisedAction Members

		/// <summary>
		/// Gets the suggested action for the target object incapsulated by this instance.
		/// </summary>
		/// <value>The suggested action.</value>
		public ProposedActions Action
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the target object of the suggested action.
		/// </summary>
		/// <value>The target object.</value>
		public Object Target
		{
			get;
			private set;
		}

		#endregion
	}
}
