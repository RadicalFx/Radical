using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace Topics.Radical.Windows.Behaviors
{
    public class Extensibility : FrameworkElement
    {
        #region Attached Property: Behaviors

        static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
                                      "Behaviors",
                                      typeof( IList<RadicalBehavior> ),
                                      typeof( Extensibility ),
                                      new PropertyMetadata( null, OnBehaviorsChanged ) );


        private static void OnBehaviorsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var oldValue = ( RadicalBehaviorCollection )e.OldValue;
            var newValue = ( RadicalBehaviorCollection )e.NewValue;

            if ( oldValue != newValue )
            {
                if ( oldValue != null && oldValue.AssociatedObject != null )
                    oldValue.Detach();
                if ( newValue != null && d != null )
                {
                    if ( newValue.AssociatedObject != null )
                        throw new InvalidOperationException( "Cannot set multiple times." );

                    newValue.Attach( ( FrameworkElement )d );
                }
            }
        }


        public static IList<RadicalBehavior> GetBehaviors( Object owner )
        {
            var fe = ( FrameworkElement )owner; 
            var behaviors = ( RadicalBehaviorCollection )fe.GetValue( BehaviorsProperty );
            if ( behaviors == null )
            {
                behaviors = new RadicalBehaviorCollection();
                fe.SetValue( BehaviorsProperty, behaviors );
            }

            return behaviors;
        }

        #endregion
    }
}
