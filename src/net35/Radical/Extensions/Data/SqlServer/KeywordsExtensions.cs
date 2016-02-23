using System.Collections.Generic;
using System;
using System.Linq;

namespace Topics.Radical.Data.SqlServer
{
    public static class KeywordsExtensions
    {
        public static IEnumerable<String> AsSqlServerKeywords( this IEnumerable<String> keywords )
        {
            return keywords.Aggregate( new List<String>(), ( acc, kw ) =>
            {
                var tmp = kw.AsSqlServerKeyword();
                acc.Add( tmp );

                return acc;
            } )
            .AsReadOnly();
        }

        public static String AsSqlServerKeyword( this String keyword )
        {
            var tmp = keyword.Replace( '*', '%' ).Replace( '?', '_' );

            return tmp;
        }
    }
}