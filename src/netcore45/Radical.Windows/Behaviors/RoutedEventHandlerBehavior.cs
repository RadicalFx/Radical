using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Behaviors
{
    public class Handle : RadicalBehavior<FrameworkElement>
    {
        #region Dependency Property: RoutedEvent

        public static readonly DependencyProperty RoutedEventProperty = DependencyProperty.Register(
            "RoutedEvent",
            typeof( String ),
            typeof( Handle ),
            new PropertyMetadata( null ) );

        public String RoutedEvent
        {
            get { return ( String )this.GetValue( RoutedEventProperty ); }
            set { this.SetValue( RoutedEventProperty, value ); }
        }

        #endregion

        #region Dependency Property: Command

        public static readonly DependencyProperty WithCommandProperty = DependencyProperty.Register(
            "WithCommand",
            typeof( ICommand ),
            typeof( Handle ),
            new PropertyMetadata( null ) );

        public ICommand WithCommand
        {
            get { return ( ICommand )this.GetValue( WithCommandProperty ); }
            set { this.SetValue( WithCommandProperty, value ); }
        }

        #endregion

        #region Dependency Property: PassingIn

        public static readonly DependencyProperty PassingInProperty = DependencyProperty.Register(
            "PassingIn",
            typeof( String ),
            typeof( Handle ),
            new PropertyMetadata( null ) );

        public String PassingIn
        {
            get { return ( String )this.GetValue( PassingInProperty ); }
            set { this.SetValue( PassingInProperty, value ); }
        }

        #endregion

        RoutedEventHandler handler = null;

        public Handle()
        {
            this.handler = ( s, e ) =>
            {
                Object args = null;
 
                if ( !String.IsNullOrWhiteSpace( this.PassingIn ) )
                {
                    Object referencedObject = null;

                    if ( this.PassingIn.StartsWith( "$args.", StringComparison.OrdinalIgnoreCase ) )
                    {
                        referencedObject = e;
                    }
                    else if ( this.PassingIn.StartsWith( "$this.", StringComparison.OrdinalIgnoreCase ) )
                    {
                        referencedObject = this.AssociatedObject;
                    }
                    else if ( this.PassingIn.StartsWith( "$originalSource.", StringComparison.OrdinalIgnoreCase ) )
                    {
                        referencedObject = e.OriginalSource;
                    }

                    if ( referencedObject != null )
                    {
                        var indexOfFirstDot = this.PassingIn.IndexOf( '.' );

                        //to do add support for nested properties Foo.Bar.Property
                        var propertyPath = this.PassingIn.Substring( indexOfFirstDot + 1 ).Split( '.' );
                        var property = propertyPath.First();

                        args = referencedObject.GetType()
                            .GetTypeInfo()
                            .GetDeclaredProperty( property )
                            .GetValue( e, null );
                    }
                    else if ( this.PassingIn.Equals( "$args", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = e;
                    }
                    else if ( this.PassingIn.Equals( "$this", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = this.AssociatedObject;
                    }
                    else if ( this.PassingIn.Equals( "$originalSource", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = e.OriginalSource;
                    }
                }

                //to do add support for AutoCommandBinding with MethodFact?
                if ( this.WithCommand.CanExecute( args ) )
                {
                    this.WithCommand.Execute( args );
                }
            };
        }

        protected override void OnAttached()
        {
            //Observable.FromEventPattern<RoutedEventArgs>(this.AssociatedObject, this.RoutedEvent)
            //    .Subscribe(se => 
            //    {
            //        handler( se.Sender, se.EventArgs);
            //    });

            //this.AssociatedObject.GetType()
            //    .GetRuntimeEvent(this.RoutedEvent)
            //    .AddEventHandler( this, handler);
        }

        protected override void OnDetached()
        {
            //this.AssociatedObject.GetType()
            //    .GetRuntimeEvent( this.RoutedEvent )
            //    .RemoveEventHandler( this, handler );
        }
    }
}
