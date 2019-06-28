namespace Radical
{
    using Radical.ComponentModel;
    using System;
    using System.Collections.Generic;

    public static class EntityCollectionExtensions
    {
        public static IEntityCollection<T> BulkLoad<T>( this IEntityCollection<T> list, IEnumerable<T> data )
            where T : class
        {
            return BulkLoad( list, data, true );
        }

        public static IEntityCollection<T> BulkLoad<T>( this IEntityCollection<T> list, IEnumerable<T> data, bool clear )
            where T : class
        {
            list.BeginInit();

            if( clear )
            {
                list.Clear();
            }

            list.AddRange( data );
            list.EndInit( true );

            return list;
        }

        public static IEntityCollection<T> BulkLoad<T, TSource>( this IEntityCollection<T> list, IEnumerable<TSource> data, Func<TSource, T> adapter )
            where T : class
        {
            list.BeginInit();
            foreach( var item in data )
            {
                list.Add( adapter( item ) );
            }
            list.EndInit( true );

            return list;
        }
    }
}
