//using System;
//using System.Diagnostics;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Controls.Primitives;
//using Topics.Radical.Validation;
//using Topics.Radical.Windows.Behaviors;

//namespace Topics.Radical.Windows.Presentation.Regions
//{
//    public static class PopupRegionBehavior
//    {
//        static readonly TraceSource logger = new TraceSource( typeof( PopupRegionBehavior ).Name );

//        static readonly RoutedEventHandler onLoaded = ( s, e ) =>
//        {
//            var fe = ( FrameworkElement )s;

//            fe.Unloaded += onUnloaded;

//            var content = PopupRegionBehavior.GetContent( fe );
//            var name = PopupRegionBehavior.GetName( fe );
//            var popup = PopupRegionBehavior.GetPopup( fe );
//            var isOpen = PopupRegionBehavior.GetIsOpen( fe );

//            if( popup == null )
//            {
//                popup = new Popup();
//                popup.Child = new ContentPresenter() { Content = content };
//                popup.IsOpen = isOpen;
//                popup.StaysOpen = false;
//                popup.Placement = PlacementMode.Bottom;
//                popup.PlacementTarget = fe;

//                popup.Closed += ( ps, pe ) => 
//                {
//                    PopupRegionBehavior.SetIsOpen( fe, false );

//                    //var bexp = fe.GetBindingExpression( IsOpenProperty );
//                    //bexp.UpdateSource();
//                };

//                PopupRegionBehavior.SetPopup( fe, popup );
//            }
//        };

//        static readonly RoutedEventHandler onUnloaded = ( s, e ) =>
//        {
//            var fe = ( FrameworkElement )s;

//            fe.Unloaded -= onUnloaded;
//        };

//        #region Attached Property: IsLoadEventAttached

//        static readonly DependencyProperty IsLoadEventAttachedProperty = DependencyProperty.RegisterAttached(
//                                      "IsLoadEventAttached",
//                                      typeof( Boolean ),
//                                      typeof( PopupRegionBehavior ),
//                                      new FrameworkPropertyMetadata( false ) );


//        static Boolean GetIsLoadEventAttached( FrameworkElement owner )
//        {
//            return ( Boolean )owner.GetValue( IsLoadEventAttachedProperty );
//        }

//        static void SetIsLoadEventAttached( FrameworkElement owner, Boolean value )
//        {
//            owner.SetValue( IsLoadEventAttachedProperty, value );
//        }

//        #endregion

//        #region Attached Property: Popup

//        static readonly DependencyProperty PopupAttachedProperty = DependencyProperty.RegisterAttached(
//                                      "Popup",
//                                      typeof( Popup ),
//                                      typeof( PopupRegionBehavior ),
//                                      new FrameworkPropertyMetadata( null ) );


//        static Popup GetPopup( FrameworkElement owner )
//        {
//            return ( Popup )owner.GetValue( PopupAttachedProperty );
//        }

//        static void SetPopup( FrameworkElement owner, Popup value )
//        {
//            owner.SetValue( PopupAttachedProperty, value );
//        }

//        #endregion

//        #region Attached Property: Content

//        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
//                                      "Content",
//                                      typeof( System.Object ),
//                                      typeof( PopupRegionBehavior ),
//                                      new FrameworkPropertyMetadata( null, OnContentChanged ) );


//        public static System.Object GetContent( DependencyObject owner )
//        {
//            return ( System.Object )owner.GetValue( ContentProperty );
//        }

//        public static void SetContent( DependencyObject owner, System.Object value )
//        {
//            owner.SetValue( ContentProperty, value );
//        }

//        #endregion

//        private static void OnContentChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//        {
//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                var fe = Ensure.That( d as FrameworkElement )
//                   .Named( "d" )
//                   .WithMessage( "This behavior can be attached only to FrameworkElement(s)." )
//                   .IsNotNull()
//                   .GetValue();

//                if( !PopupRegionBehavior.GetIsLoadEventAttached( fe ) )
//                {
//                    fe.Loaded += onLoaded;
//                    PopupRegionBehavior.SetIsLoadEventAttached( fe, true );
//                }
//            }
//        }

//        #region Attached Property: IsOpen

//        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached(
//                                      "IsOpen",
//                                      typeof( Boolean ),
//                                      typeof( PopupRegionBehavior ),
//                                      new FrameworkPropertyMetadata( false, OnIsOpenChanged ) );


//        public static Boolean GetIsOpen( DependencyObject owner )
//        {
//            return ( Boolean )owner.GetValue( IsOpenProperty );
//        }

//        public static void SetIsOpen( DependencyObject owner, Boolean value )
//        {
//            owner.SetValue( IsOpenProperty, value );
//        }

//        #endregion

//        private static void OnIsOpenChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//        {
//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                var fe = Ensure.That( d as FrameworkElement )
//                   .Named( "d" )
//                   .WithMessage( "This behavior can be attached only to FrameworkElement(s)." )
//                   .IsNotNull()
//                   .GetValue();

//                if( !PopupRegionBehavior.GetIsLoadEventAttached( fe ) )
//                {
//                    fe.Loaded += onLoaded;
//                    PopupRegionBehavior.SetIsLoadEventAttached( fe, true );
//                }

//                var popup = GetPopup( fe );
//                if( popup != null && e.NewValue is Boolean )
//                {
//                    popup.IsOpen = ( Boolean )e.NewValue;
//                }
//            }
//        }

//        #region Attached Property: Name

//        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
//                                      "Name",
//                                      typeof( String ),
//                                      typeof( PopupRegionBehavior ),
//                                      new FrameworkPropertyMetadata( null, OnNameChanged ) );


//        public static String GetName( DependencyObject owner )
//        {
//            return ( String )owner.GetValue( NameProperty );
//        }

//        public static void SetName( DependencyObject owner, String value )
//        {
//            owner.SetValue( NameProperty, value );
//        }

//        #endregion

//        private static void OnNameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//        {
//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                var fe = Ensure.That( d as FrameworkElement )
//                   .Named( "d" )
//                   .WithMessage( "This behavior can be attached only to FrameworkElement(s)." )
//                   .IsNotNull()
//                   .GetValue();

//                if( !PopupRegionBehavior.GetIsLoadEventAttached( fe ) )
//                {
//                    fe.Loaded += onLoaded;
//                    PopupRegionBehavior.SetIsLoadEventAttached( fe, true );
//                }
//            }
//        }
//    }
//}
