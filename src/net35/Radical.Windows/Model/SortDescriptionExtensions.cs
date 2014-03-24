//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;
//using Topics.Radical.ComponentModel;

//namespace Topics.Radical.Windows.Model
//{
//    static class SortDescriptionExtensions
//    {
//        public static ListSortDescription Convert<T>( this SortDescription item )// where T : class
//        {
//            var property = new EntityItemViewPropertyDescriptor<T>
//            (
//                typeof( T ).GetProperty( item.PropertyName )
//            );

//            var convertedItem = new ListSortDescription( property, item.Direction );

//            return convertedItem;
//        }

//        public static ListSortDescriptionCollection Convert<T>( this SortDescriptionCollection source ) //where T : class
//        {
//            var tmp = source.Aggregate( new List<ListSortDescription>(), ( a, sd ) =>
//            {
//                a.Add( sd.Convert<T>() );
//                return a;
//            } )
//            .ToArray();

//            var list = new ListSortDescriptionCollection( tmp );
//            return list;
//        }
//    }
//}
