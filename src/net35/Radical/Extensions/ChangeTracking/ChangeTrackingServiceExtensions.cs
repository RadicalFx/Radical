using System;
using System.Linq;
using Topics.Radical.ComponentModel.ChangeTracking;
using System.Collections.Generic;
using Topics.Radical.Conversions;
using Topics.Radical.Reflection;

namespace Topics.Radical.ChangeTracking
{
	/// <summary>
	/// Adds behaviors to an <see cref="IChangeTrackingService"/>.
	/// </summary>
	public static class ChangeTrackingServiceExtensions
	{
        static IEnumerable<T> GetItemsWhereActionIs<T>( this IChangeTrackingService service, ProposedActions filter )
        {
            var items = service.GetAdvisory()
                .Where( a =>
                {
                    return a.Target.GetType().Is<T>() && a.Action == filter;
                } )
                .Select( a => a.Target )
                .Cast<T>();

            return items;
        }

        /// <summary>
        /// Get the items that has been created.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="service">The service.</param>
        /// <returns>The requested list.</returns>
        public static IEnumerable<T> GetNewItems<T>( this IChangeTrackingService service ) 
        {
            return service.GetItemsWhereActionIs<T>( ProposedActions.Create );
        }

        /// <summary>
        /// Get the items that has been modified.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="service">The service.</param>
        /// <returns>The requested list.</returns>
        public static IEnumerable<T> GetChangedItems<T>( this IChangeTrackingService service )
        {
            return service.GetItemsWhereActionIs<T>( ProposedActions.Update );
        }

        /// <summary>
        /// Get the items that has been deleted.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="service">The service.</param>
        /// <returns>The requested list.</returns>
        public static IEnumerable<T> GetDeletedItems<T>( this IChangeTrackingService service )
        {
            return service.GetItemsWhereActionIs<T>( ProposedActions.Delete );
        }

        /// <summary>
        /// Get the items that has been deleted but being marked
        /// as transient is a nonsense to remove them from the
        /// underlying storage, simply dispose its instance if required.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="service">The service.</param>
        /// <returns>The requested list.</returns>
        public static IEnumerable<T> GetRemovedItems<T>( this IChangeTrackingService service )
        {
            return service.GetItemsWhereActionIs<T>( ProposedActions.Dispose );
        }

		/// <summary>
		/// Attaches the specified source entity to the change tracking service.
		/// </summary>
		/// <typeparam name="T">The type of the entity.</typeparam>
		/// <param name="service">The service to attach the entity to.</param>
		/// <param name="source">The source entity.</param>
		/// <returns>The attached entity.</returns>
		public static T Attach<T>( this IChangeTrackingService service, T source )
		{
			source.As<IMemento>( m => 
			{
				service.Attach( m );
			}, () => 
			{
				throw new NotSupportedException( "Only IMemento enties can be attached to an IChangeTrackingService." );
			} );

			return source;
		}

		/// <summary>
		/// Attaches the specified list of entities to the change tracking service.
		/// </summary>
		/// <typeparam name="T">The type of the entity.</typeparam>
		/// <param name="service">The service to attach the entity to.</param>
		/// <param name="data">The source entity list.</param>
		/// <returns>The attached entity list.</returns>
		public static IEnumerable<T> Attach<T>( this IChangeTrackingService service, IEnumerable<T> data )
		{
			foreach( var element in data )
			{
				service.Attach( element );
			}

			return data;
		}
	}
}
