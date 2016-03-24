namespace Topics.Radical.Data.Adapters
{
    using System;

    public abstract class DataFiller<TSource, TDestination> : IDataFiller<TSource, TDestination>
    {
        protected DataFiller()
        {
 
        }

        public virtual void Invalidate()
        {
            //NOP
        }

        public abstract TDestination Fill( TSource source, TDestination destination );
    }
}
