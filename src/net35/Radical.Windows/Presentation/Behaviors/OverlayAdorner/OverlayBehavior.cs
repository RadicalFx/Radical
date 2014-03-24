using System;
using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Media;

namespace Topics.Radical.Windows.Behaviors
{
    public class OverlayBehavior : RadicalBehavior<FrameworkElement>
    {
        #region Dependency Property: Content

        /// <summary>
        /// Content Dependency property
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof( System.Object ),
            typeof( OverlayBehavior ),
            new PropertyMetadata( null ) );

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public System.Object Content
        {
            get { return ( System.Object )this.GetValue( ContentProperty ); }
            set { this.SetValue( ContentProperty, value ); }
        }

        #endregion

        #region Dependency Property: IsVisible

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible",
            typeof( Boolean ),
            typeof( OverlayBehavior ),
            new PropertyMetadata( true, ( s, e ) =>
            {
                ( ( OverlayBehavior )s ).OnIsVisibleChanged( e );
            } ) );

        public Boolean IsVisible
        {
            get { return ( Boolean )this.GetValue( IsVisibleProperty ); }
            set { this.SetValue( IsVisibleProperty, value ); }
        }

        #endregion

        #region Dependency Property: DisableAdornedElement

        public static readonly DependencyProperty DisableAdornedElementProperty = DependencyProperty.Register(
            "DisableAdornedElement",
            typeof( Boolean ),
            typeof( OverlayBehavior ),
            new PropertyMetadata( false ) );

        public Boolean DisableAdornedElement
        {
            get { return ( Boolean )this.GetValue( DisableAdornedElementProperty ); }
            set { this.SetValue( DisableAdornedElementProperty, value ); }
        }

        #endregion

        #region Dependency Property: IsHitTestVisible

        public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyProperty.Register(
            "IsHitTestVisible",
            typeof( Boolean ),
            typeof( OverlayBehavior ),
            new PropertyMetadata( true ) );

        public Boolean IsHitTestVisible
        {
            get { return ( Boolean )this.GetValue( IsHitTestVisibleProperty ); }
            set { this.SetValue( IsHitTestVisibleProperty, value ); }
        }

        #endregion

        #region Dependency Property: Background

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof( Brush ),
            typeof( OverlayBehavior ),
            new PropertyMetadata( null, ( s, e ) =>
            {
                ( ( OverlayBehavior )s ).OnBackgroundChanged( e );
            } ) );

        public Brush Background
        {
            get { return ( Brush )this.GetValue( BackgroundProperty ); }
            set { this.SetValue( BackgroundProperty, value ); }
        }

        #endregion

        //#region Dependency Property: BackgroundOpacity

        //public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
        //    "BackgroundOpacity",
        //    typeof( Double ),
        //    typeof( OverlayBehavior ),
        //    new PropertyMetadata( 1d, ( s, e ) =>
        //    {
        //        ( ( OverlayBehavior )s ).OnBackgroundOpacityChanged( e );
        //    } ) );

        //public Double BackgroundOpacity
        //{
        //    get { return ( Double )this.GetValue( BackgroundOpacityProperty ); }
        //    set { this.SetValue( BackgroundOpacityProperty, value ); }
        //}

        //#endregion

        private void OnBackgroundChanged( DependencyPropertyChangedEventArgs e )
        {
            if ( this.isAdornerVisible ) 
            {
                this.adorner.InvalidateVisual();
            }
        }

        private void OnIsVisibleChanged( DependencyPropertyChangedEventArgs e )
        {
            this.Toggle();
        }

        //private void OnBackgroundOpacityChanged( DependencyPropertyChangedEventArgs e )
        //{
        //    if ( this.isAdornerVisible )
        //    {
        //        this.adorner.InvalidateVisual();
        //    }
        //}

        ContentOverlayAdorner adorner;
        Boolean isAdornerVisible;
        private bool wasEnabled;

        protected override void OnAttached()
        {
            base.OnAttached();

            if ( this.Content != null )
            {
                this.AssociatedObject.Loaded += new RoutedEventHandler( OnLoaded );
            }
        }

        void OnLoaded( object sender, RoutedEventArgs e )
        {
            if ( this.IsVisible )
            {
                this.ShowAdorner();
            }
        }

        private void Toggle()
        {
            if ( this.IsVisible )
            {
                this.ShowAdorner();
            }
            else
            {
                this.HideAdorner();
            }
        }

        void ShowAdorner()
        {
            if ( !this.isAdornerVisible )
            {
                var layer = AdornerLayer.GetAdornerLayer( this.AssociatedObject );
                Debug.WriteLineIf( layer == null, "Overlay: cannot find any AdornerLayer on the given element." );

                if ( layer != null )
                {
                    this.adorner = new ContentOverlayAdorner( this.AssociatedObject, this.Content )
                    {
                        IsHitTestVisible = this.IsHitTestVisible,
                        Background = this.Background
                    };
                    layer.Add( this.adorner );

                    this.wasEnabled = this.AssociatedObject.IsEnabled;
                    this.AssociatedObject.IsEnabled = !this.DisableAdornedElement;

                    this.isAdornerVisible = true;
                }
            }
        }

        void HideAdorner()
        {
            if ( this.isAdornerVisible && this.adorner != null )
            {
                var layer = AdornerLayer.GetAdornerLayer( this.AssociatedObject );
                Debug.WriteLineIf( layer == null, "Overlay: cannot find any AdornerLayer on the given element." );

                if ( layer != null )
                {
                    layer.Remove( this.adorner );
                    this.adorner = null;
                    this.isAdornerVisible = false;
                }

                this.AssociatedObject.IsEnabled = this.wasEnabled;
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= new RoutedEventHandler( OnLoaded );
            this.HideAdorner();

            base.OnDetaching();
        }
    }
}
