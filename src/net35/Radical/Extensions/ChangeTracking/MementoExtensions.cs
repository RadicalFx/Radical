namespace Topics.Radical.ChangeTracking
{
    using System;
    using Topics.Radical.ComponentModel.ChangeTracking;

    /// <summary>
    /// Extends <c>IMemento</c> interface providing shortcuts
    /// to inspect the state of the object against its memento.
    /// </summary>
    public static class MementoExtensions
    {
        /// <summary>
        /// Determines whether the specified entity is transient.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the supplied entity is not tracked by a change tracking service.</exception>
        public static Boolean IsTransient( this IMemento entity )
        {
            var memento = entity.Memento;
            if( memento == null )
            {
                throw new ArgumentException();
            }

            var state = memento.GetEntityState( entity );

            return ( state & EntityTrackingStates.IsTransient ) == EntityTrackingStates.IsTransient;
        }

        /// <summary>
        /// Determines whether the specified entity is changed.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is changed; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the supplied entity is not tracked by a change tracking service.</exception>
        public static Boolean IsChanged( this IMemento entity )
        {
            var memento = entity.Memento;
            if( memento == null )
            {
                throw new ArgumentException();
            }

            var state = memento.GetEntityState( entity );

            return ( state & EntityTrackingStates.HasBackwardChanges ) == EntityTrackingStates.HasBackwardChanges;
        }
    }
}
