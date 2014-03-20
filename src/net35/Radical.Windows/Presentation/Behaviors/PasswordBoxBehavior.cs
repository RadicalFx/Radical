using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Topics.Radical.Windows.Input;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Behaviors
{
    public class PasswordBoxBehavior : RadicalBehavior<PasswordBox>, ICommandSource
    {
        #region Dependency Property: Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof( String ),
            typeof( PasswordBoxBehavior ),
            new FrameworkPropertyMetadata( null, ( s, e ) => ( ( PasswordBoxBehavior )s ).OnTextChanged( ( String )e.NewValue ) )
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                BindsTwoWayByDefault = true,
            } );

        public String Text
        {
            get { return ( String )this.GetValue( TextProperty ); }
            set { this.SetValue( TextProperty, value ); }
        }

        #endregion

        #region Dependency Property: Command

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof( ICommand ),
            typeof( PasswordBoxBehavior ),
            new PropertyMetadata( null ) );

        public ICommand Command
        {
            get { return ( ICommand )this.GetValue( CommandProperty ); }
            set { this.SetValue( CommandProperty, value ); }
        }

        #endregion

        public object CommandParameter
        {
            get;
            set;
        }

        public IInputElement CommandTarget
        {
            get { return this.AssociatedObject; }
        }

        KeyEventHandler onPreviewKeyDown;
        RoutedEventHandler onPasswordChanged;

        //true if going from password box to view model
        private bool isPushing;

        public PasswordBoxBehavior()
        {
            this.onPasswordChanged = ( s, e ) =>
            {
                this.isPushing = true;

                this.Text = this.AssociatedObject.Password;

                var text = BindingOperations.GetBindingExpression( this, PasswordBoxBehavior.TextProperty );
                var tag = BindingOperations.GetBindingExpression( this.AssociatedObject, PasswordBox.TagProperty );
                if ( text.HasError )
                {
                    System.Windows.Controls.Validation.MarkInvalid( tag, text.ValidationError );
                }
                else
                {
                    System.Windows.Controls.Validation.ClearInvalid( tag );
                }

                this.isPushing = false;
            };

            onPreviewKeyDown = ( s, e ) =>
            {
                var d = ( DependencyObject )s;

                if ( this.Command != null )
                {
                    var cmd = this.Command;
                    var prm = this.CommandParameter;

                    var gestures = cmd.GetGestures();
                    var senderGestures = gestures.Where( gesture => gesture.Matches( d, e ) );

                    if ( ( ( gestures.None() && e.Key == System.Windows.Input.Key.Enter ) || senderGestures.Any() ) && cmd.CanExecute( prm ) )
                    {
                        var k = e.Key;
                        var m = ModifierKeys.None;

                        if ( senderGestures.Any() )
                        {
                            var gesture = senderGestures.First();
                            var keygesture = gesture as KeyGesture;
                            if ( keygesture != null )
                            {
                                k = keygesture.Key;
                                m = keygesture.Modifiers;
                            }
                        }

                        var args = new PasswordBoxCommandArgs( k, m, prm );
                        cmd.Execute( args );
                        e.Handled = true;
                    }
                }
            };
        }

        void OnTextChanged( String newValue )
        {
            if ( !this.isPushing )
            {
                this.AssociatedObject.PasswordChanged -= this.onPasswordChanged;
                this.AssociatedObject.Password = newValue;
                this.AssociatedObject.PasswordChanged += this.onPasswordChanged;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PasswordChanged += this.onPasswordChanged;
            this.AssociatedObject.PreviewKeyDown += this.onPreviewKeyDown;

            BindingOperations.SetBinding( this.AssociatedObject, PasswordBox.TagProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath( TextProperty.Name ),
                //Mode = BindingMode.OneWay
            } );
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PasswordChanged -= this.onPasswordChanged;
            this.AssociatedObject.PreviewKeyDown -= this.onPreviewKeyDown;

            base.OnDetaching();
        }
    }

    public class PasswordBoxCommandArgs : EventArgs
    {
        public PasswordBoxCommandArgs( System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifiers, Object commandParameter )
        {
            this.Key = key;
            this.Modifiers = modifiers;
            this.CommandParameter = commandParameter;
        }

        public System.Windows.Input.Key Key { get; private set; }
        public System.Windows.Input.ModifierKeys Modifiers { get; private set; }

        public Object CommandParameter { get; private set; }
    }
}
