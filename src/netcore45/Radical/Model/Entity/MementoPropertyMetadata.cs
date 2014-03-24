using System;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.ComponentModel.ChangeTracking;

namespace Topics.Radical.Model
{
	//public static class MementoPropertyMetadataBuilder
	//{
	//    public class TypedMementoPropertyMetadataBuilder<T>
	//    {
	//        public MementoPropertyMetadata<TValue> And<TValue>( Expression<Func<T, TValue>> property )
	//        {
	//            var name = property.GetMemberName();
	//            return new MementoPropertyMetadata<TValue>( name );
	//        }
	//    }

	//    public static TypedMementoPropertyMetadataBuilder<T> For<T>()
	//    {
	//        return new TypedMementoPropertyMetadataBuilder<T>();
	//    }
	//}

	public static class MementoPropertyMetadata 
	{
		public static MementoPropertyMetadata<T> Create<T>( Expression<Func<T>> property ) 
		{
			return new MementoPropertyMetadata<T>( property );
		}

		public static MementoPropertyMetadata<T> Create<T>( String propertyName )
		{
			return new MementoPropertyMetadata<T>( propertyName );
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">The type of the property.</typeparam>
	public class MementoPropertyMetadata<T> : PropertyMetadata<T>, 
		IMementoPropertyMetadata
	{
		public MementoPropertyMetadata( String propertyName )
			: base( propertyName )
		{
			this.TrackChanges = true;
		}

		public MementoPropertyMetadata( Expression<Func<T>> property )
			: this( property.GetMemberName() )
		{

		}

		public Boolean TrackChanges { get; set; }

		public MementoPropertyMetadata<T> DisableChangesTracking()
		{
			this.TrackChanges = false;
			return this;
		}

		public MementoPropertyMetadata<T> EnableChangesTracking()
		{
			this.TrackChanges = true;
			return this;
		}
	}
}
