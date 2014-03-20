using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Model;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Behaviors
{
    class RadicalBehaviorCollection : FrameworkElement, IList<RadicalBehavior>
    {
        class RadicalBehaviorCollectionHolder : EntityCollection<RadicalBehavior> 
        {
            private RadicalBehaviorCollection radicalBehaviorCollection;

            public RadicalBehaviorCollectionHolder( RadicalBehaviorCollection radicalBehaviorCollection )
            {
                // TODO: Complete member initialization
                this.radicalBehaviorCollection = radicalBehaviorCollection;
            }
            protected override void OnAddCompleted( int index, RadicalBehavior value )
            {
                base.OnAddCompleted( index, value );

                if ( value.AssociatedObject == null )
                {
                    value.Attach( this.radicalBehaviorCollection.AssociatedObject );
                }
            }

            protected override void OnRemoveCompleted( RadicalBehavior value, int index )
            {
                base.OnRemoveCompleted( value, index );

                if ( this.radicalBehaviorCollection.AssociatedObject != null )
                {
                    value.Detach();
                }

            }
        }

        readonly RadicalBehaviorCollectionHolder holder;

        public RadicalBehaviorCollection()
        {
            this.holder = new RadicalBehaviorCollectionHolder( this );
        }

        internal FrameworkElement AssociatedObject { get; private set; }

        internal void Detach()
        {
            foreach ( var bhv in this )
            {
                bhv.Detach();
            }

            this.AssociatedObject = null;
        }

        internal void Attach( FrameworkElement d )
        {
            this.AssociatedObject = d;

            foreach ( var bhv in this )
            {
                bhv.Attach( d );
            }
        }

        int IList<RadicalBehavior>.IndexOf( RadicalBehavior item )
        {
            return this.holder.IndexOf( item );
        }

        void IList<RadicalBehavior>.Insert( int index, RadicalBehavior item )
        {
            this.holder.Insert( index, item );
        }

        void IList<RadicalBehavior>.RemoveAt( int index )
        {
            this.holder.RemoveAt( index );
        }

        RadicalBehavior IList<RadicalBehavior>.this[ int index ]
        {
            get { return this.holder[ index ]; }
            set { this.holder[ index ] = value; }
        }

        void ICollection<RadicalBehavior>.Add( RadicalBehavior item )
        {
            this.holder.Add( item );
        }

        void ICollection<RadicalBehavior>.Clear()
        {
            this.holder.Clear();
        }

        bool ICollection<RadicalBehavior>.Contains( RadicalBehavior item )
        {
            return this.holder.Contains( item );
        }

        void ICollection<RadicalBehavior>.CopyTo( RadicalBehavior[] array, int arrayIndex )
        {
            this.holder.CopyTo( array, arrayIndex );
        }

        int ICollection<RadicalBehavior>.Count
        {
            get { return this.holder.Count; }
        }

        bool ICollection<RadicalBehavior>.IsReadOnly
        {
            get { return (( ICollection<RadicalBehavior> )this.holder).IsReadOnly; }
        }

        bool ICollection<RadicalBehavior>.Remove( RadicalBehavior item )
        {
            return this.holder.Remove( item );
        }

        IEnumerator<RadicalBehavior> IEnumerable<RadicalBehavior>.GetEnumerator()
        {
            return this.holder.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.holder.GetEnumerator();
        }
    }
}
