//extern alias tpx;

using Radical.ComponentModel.ChangeTracking;
using Radical.Model;

namespace Radical.Tests.ChangeTracking
{

    class PersonCollection : MementoEntityCollection<Person>
    {
        public PersonCollection(IChangeTrackingService memento)
            : base()
        {
            Memento = memento;
        }
    }
}
