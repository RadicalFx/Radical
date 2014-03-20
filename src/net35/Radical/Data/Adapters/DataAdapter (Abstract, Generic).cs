namespace Topics.Radical.Data.Adapters
{
	using System;

	public abstract class DataAdapter<TSource, TDestination> : IDataAdapter<TSource, TDestination>
	{
		protected DataAdapter()
		{
 
		}

		public virtual void Invalidate()
		{
			//NOP
		}

		public abstract TDestination Adapt( TSource source );
	}
}
