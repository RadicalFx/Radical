using System;
using System.Collections.Generic;
using Topics.Radical.ComponentModel.QueryModel;
using Topics.Radical.Validation;

namespace Topics.Radical.Model.QueryModel
{
    /// <summary>
    /// Defines a query to retrieve all entities of the given type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AllEntitiesQuery<TSource, TResult> : IQuerySpecification<TSource, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllEntitiesQuery&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        public AllEntitiesQuery()
        {
            
        }
    }
}
