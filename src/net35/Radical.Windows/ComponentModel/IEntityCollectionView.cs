namespace Topics.Radical.ComponentModel
{
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;

	public interface IEntityCollectionView<T> : 
		IEntityView<T>, 
		INotifyCollectionChanged, 
		ICollectionView,
		/* IEditableCollectionView, */
		ICollectionViewFactory
	{

	}
}