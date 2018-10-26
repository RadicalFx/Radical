using System;
using System.ComponentModel;

namespace Radical.Tests.Model.Entity
{
    interface IMockEntity : INotifyPropertyChanged //, ISelfTrackingEntity
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        Int32 Number { get; set; }

        String MainProperty { get; set; }
        String SubProperty { get; }

        void SetInitialValue<T>( System.Linq.Expressions.Expression<Func<T>> property, T value );
    }
}
