//extern alias tpx;

using Radical.Model;
using Radical.ComponentModel.ChangeTracking;

namespace Radical.Tests.ChangeTracking
{

    class PersonCollection : MementoEntityCollection<Person>
    {
        public PersonCollection( IChangeTrackingService memento )
            : base()
        {
            this.Memento = memento;
        }
    }
}
