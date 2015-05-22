using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.Windows.Behaviors;

namespace Topics.Radical.Windows.Behaviors
{
    public class Handle : RadicalBehavior<FrameworkElement>
    {
        #region Dependency Property: RoutedEvent

        public static readonly DependencyProperty RoutedEventProperty = DependencyProperty.Register(
            "RoutedEvent",
            typeof( RoutedEvent ),
            typeof( Handle ),
            new PropertyMetadata( null ) );

        public RoutedEvent RoutedEvent
        {
            get { return ( RoutedEvent )this.GetValue( RoutedEventProperty ); }
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

#if FX35
                if ( !String.IsNullOrEmpty( this.PassingIn ) )
#else 
                if ( !String.IsNullOrWhiteSpace( this.PassingIn ) )
#endif
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
                    else if ( this.PassingIn.StartsWith( "$source.", StringComparison.OrdinalIgnoreCase ) )
                    {
                        referencedObject = e.Source;
                    }
                    else if ( this.PassingIn.StartsWith( "$originalSource.", StringComparison.OrdinalIgnoreCase ) )
                    {
                        referencedObject = e.OriginalSource;
                    }

                    if ( referencedObject != null )
                    {
                        var indexOfFirstDot = this.PassingIn.IndexOf( '.' );

                        //TODO: add support for nested properties Foo.Bar.Property
                        var propertyPath = this.PassingIn.Substring( indexOfFirstDot + 1 ).Split( '.' );
                        var property = propertyPath.First();

                        args = referencedObject.GetType().GetProperty(property).GetValue( referencedObject, null);
                    }
                    else if ( this.PassingIn.Equals( "$args", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = e;
                    }
                    else if ( this.PassingIn.Equals( "$this", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = this.AssociatedObject;
                    }
                    else if ( this.PassingIn.Equals( "$source", StringComparison.OrdinalIgnoreCase ) )
                    {
                        args = e.Source;
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
            base.OnAttached();

            this.AssociatedObject.AddHandler( this.RoutedEvent, handler );
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.RemoveHandler( this.RoutedEvent, handler );
        }
    }
}
