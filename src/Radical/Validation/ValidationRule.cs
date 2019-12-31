using System;
using System.Linq.Expressions;

namespace Radical.Validation
{
    class ValidationRule<T>
    {
        public Func<ValidationContext<T>, ValidationResult> Rule { get; set; }
        public Expression<Func<T, object>> Property { get; set; }
    }
}
