using System.Collections.Generic;
using System.Linq;

namespace Radical.Data.SqlServer
{
    public static class KeywordsExtensions
    {
        public static IEnumerable<string> AsSqlServerKeywords(this IEnumerable<string> keywords)
        {
            return keywords.Aggregate(new List<string>(), (acc, kw) =>
           {
               var tmp = kw.AsSqlServerKeyword();
               acc.Add(tmp);

               return acc;
           })
            .AsReadOnly();
        }

        public static string AsSqlServerKeyword(this string keyword)
        {
            var tmp = keyword.Replace('*', '%').Replace('?', '_');

            return tmp;
        }
    }
}