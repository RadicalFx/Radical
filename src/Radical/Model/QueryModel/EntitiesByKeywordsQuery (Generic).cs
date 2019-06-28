using Radical.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Model.QueryModel
{
    /// <summary>
    /// Defines a query to retrive entities given a list of keywords.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class EntitiesByKeywordsQuery<TSource, TResult> : AbstractQuerySpecification<TSource, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesByKeywordsQuery&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        public EntitiesByKeywordsQuery( IEnumerable<string> keywords )
        {
            Ensure.That( keywords ).Named( "keywords" ).IsNotNull();

            this.Keywords = keywords;
        }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        public IEnumerable<string> Keywords
        {
            get;
            private set;
        }

        private string value = null;
        /// <summary>
        /// Returns a <see cref="System.string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if( value == null )
            {
                value = this.Keywords.Aggregate( new StringBuilder(), ( a, w ) =>
                {
                    a.AppendFormat( "{0}, ", w );

                    return a;
                } )
                .ToString()
                .TrimEnd( new[] { ',', ' ' } );
            }

            return value;
        }
    }
}
