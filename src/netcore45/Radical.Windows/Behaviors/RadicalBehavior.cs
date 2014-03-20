using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
using Topics.Radical.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Topics.Radical.Windows.Behaviors
{
    public abstract class RadicalBehavior : FrameworkElement
    {
        protected internal FrameworkElement AssociatedObject { get; private set; }

        protected abstract void OnAttached();

        protected abstract void OnDetached();

        internal void Detach()
        {
            this.OnDetached();
        }

        internal void Attach( FrameworkElement d )
        {
            this.AssociatedObject = d;

            //var b = new Binding()
            //{
            //    Source = this.AssociatedObject,
            //    Path = new PropertyPath( "DataContext" ),
            //    Mode = BindingMode.TwoWay,
            //};

            //this.SetBinding( FrameworkElement.DataContextProperty, b );

            this.OnAttached();
        }
    }

    public abstract class RadicalBehavior<T> : RadicalBehavior where T : FrameworkElement
    {
        protected new T AssociatedObject
        {
            get { return ( T )base.AssociatedObject; }
        }
    }
}
