//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;

//namespace Topics.Radical.Windows.Model
//{
//    static class ListSortDescriptionExtensions
//    {
//        public static SortDescription Convert( this ListSortDescription item )
//        {
//            var convertedItem = new SortDescription
//            (
//                item.PropertyDescriptor.Name,
//                item.SortDirection
//            );

//            return convertedItem;
//        }

//        public static SortDescriptionCollection Convert( this ListSortDescriptionCollection source )
//        {
//            var list = source.Cast<ListSortDescription>()
//                .Aggregate( new SortDescriptionCollection(), ( a, lsd ) =>
//                {
//                    a.Add( lsd.Convert() );
//                    return a;
//                } );

//            return list;
//        }
//    }
//}
