using Radical.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Radical.Linq
{
    public static class ListExtensions
    {
        /// <summary>
        /// Syncs the specified source list with the given destination list.
        /// The sync direction is from source to destination.
        /// </summary>
        /// <typeparam name="T">The trype of the list item.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="destination">The destination list.</param>
        /// <returns>A reference to the destination list.</returns>
        public static IList<T> Sync<T>( this IList<T> source, IList<T> destination )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            Ensure.That( destination ).Named( "destination" ).IsNotNull();

            /*
             * Aggiungiamo gli elementi nuovi.
             */
            foreach( var item in source.Except( destination ).ToArray() )
            {
                destination.Insert( source.IndexOf( item ), item );
            }

            /*
             * Dobbiamo rimuovere quelli cancellati.
             */
            foreach( var item in destination.Except( source ).ToArray() )
            {
                destination.Remove( item );
            }

            /*
             * A questo punto sono identiche come contenuto ma dobbiamo 
             * ancora sincronizzare le posizioni che potrebbero essere 
             * diverse.
             */
            foreach( var item in source )
            {
                var sourceIndex = source.IndexOf( item );
                var destIndex = destination.IndexOf( item );

                if( sourceIndex != destIndex )
                {
                    var obj = destination[ destIndex ];
                    destination.RemoveAt( destIndex );
                    destination.Insert( sourceIndex, obj );
                }
            }

            return destination;
        }
    }
}
