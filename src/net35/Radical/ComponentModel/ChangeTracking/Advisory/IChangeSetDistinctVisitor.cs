namespace Topics.Radical.ComponentModel.ChangeTracking
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// A class that implements <c>IChangeSetDistinctVisitor</c> is requested to analyze the
	/// supplied change set and return a list (a distinct list) of changed entities with the most
	/// significant change occurred to the entity. An <c>IChangeSetDistinctVisitor</c> is required
	/// in order to implement a custom advisory provisioning angine.
	/// </summary>
	public interface IChangeSetDistinctVisitor
	{
		/// <summary>
		/// Visits the specified change set.
		/// </summary>
		/// <param name="changeSet">The change set to visit.</param>
		/// <returns>A distinct dictionary containing a reference to all the changed entities and the most important change.</returns>
		IDictionary<Object, IChange> Visit( IChangeSet changeSet );
	}
}
