using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Topics.Radical.Windows.Behaviors
{
    public static class TextBoxManager
    {
        #region Attached Property: IsAttached

        static readonly DependencyProperty IsAttachedProperty = DependencyProperty.RegisterAttached(
                                      "IsAttached",
                                      typeof( Boolean ),
                                      typeof( TextBoxManager ),
                                      new PropertyMetadata( false ) );


        static Boolean GetIsAttached( DependencyObject owner )
        {
            return ( Boolean )owner.GetValue( IsAttachedProperty );
        }

        static void SetIsAttached( DependencyObject owner, Boolean value )
        {
            owner.SetValue( IsAttachedProperty, value );
        }

        #endregion

        #region Attached Property: Text

        public static readonly DependencyProperty BindableTextProperty = DependencyProperty.RegisterAttached(
                                      "BindableText",
                                      typeof( String ),
                                      typeof( TextBoxManager ),
                                      new PropertyMetadata( null, OnBindableTextChanged ) );


        public static String GetBindableText( TextBox owner )
        {
            return ( String )owner.GetValue( BindableTextProperty );
        }

        public static void SetBindableText( TextBox owner, String value )
        {
            owner.SetValue( BindableTextProperty, value );
        }

        #endregion

        private static void OnBindableTextChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var txt = d as TextBox;
            var newValue = e.NewValue as String;
            if ( txt != null )
            {
                if ( !GetIsAttached( txt ) )
                {
                    txt.TextChanged += ( s, args ) =>
                    {
                        var sender = ( TextBox )s;
                        SetBindableText( sender, sender.Text );
                    };

                    SetIsAttached( txt, true );
                }

                if ( txt.Text != newValue )
                {
                    txt.Text = newValue;
                }
            }
        }
    }
}
