using System;
using System.ComponentModel;

namespace Radical.Tests.Model.Entity
{
    interface IMockEntity : INotifyPropertyChanged //, ISelfTrackingEntity
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        int Number { get; set; }

        string MainProperty { get; set; }
        string SubProperty { get; }

        void SetInitialValue<T>( System.Linq.Expressions.Expression<Func<T>> property, T value );
    }
}
