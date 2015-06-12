using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Topics.Radical.Validation;
using System.Windows.Controls;

namespace Topics.Radical.Windows.Behaviors
{
    public static class DragDropManager
    {
        #region Attached Property: IsDragSourceAttached

        public static readonly DependencyProperty IsDragSourceAttachedProperty = DependencyProperty.RegisterAttached(
                                      "IsDragSourceAttached",
                                      typeof( Boolean ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( false ) );


        static Boolean GetIsDragSourceAttached( DependencyObject owner )
        {
            return ( Boolean )owner.GetValue( IsDragSourceAttachedProperty );
        }

        static void SetIsDragSourceAttached( DependencyObject owner, Boolean value )
        {
            owner.SetValue( IsDragSourceAttachedProperty, value );
        }

        #endregion

        #region Attached Property: IsDropTargetAttached

        public static readonly DependencyProperty IsDropTargetAttachedProperty = DependencyProperty.RegisterAttached(
                                      "IsDropTargetAttached",
                                      typeof( Boolean ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( false ) );


        static Boolean GetIsDropTargetAttached( DependencyObject owner )
        {
            return ( Boolean )owner.GetValue( IsDropTargetAttachedProperty );
        }

        static void SetIsDropTargetAttached( DependencyObject owner, Boolean value )
        {
            owner.SetValue( IsDropTargetAttachedProperty, value );
        }

        #endregion

        #region Attached Property: DropTarget

        public static readonly DependencyProperty DropTargetProperty = DependencyProperty.RegisterAttached(
                                      "DropTarget",
                                      typeof( Object ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null ) );


        public static Object GetDropTarget( DependencyObject owner )
        {
            return ( Object )owner.GetValue( DropTargetProperty );
        }

        public static void SetDropTarget( DependencyObject owner, Object value )
        {
            owner.SetValue( DropTargetProperty, value );
        }

        #endregion

        #region Attached Property: DataObjectType

        public static readonly DependencyProperty DataObjectTypeProperty = DependencyProperty.RegisterAttached(
                                      "DataObjectType",
                                      typeof( String ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null ) );


        public static String GetDataObjectType( DependencyObject owner )
        {
            return ( String )owner.GetValue( DataObjectTypeProperty );
        }

        public static void SetDataObjectType( DependencyObject owner, String value )
        {
            owner.SetValue( DataObjectTypeProperty, value );
        }

        #endregion

        #region Attached Property: DataObject

        public static readonly DependencyProperty DataObjectProperty = DependencyProperty.RegisterAttached(
                                      "DataObject",
                                      typeof( Object ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null, OnDataObjectChanged ) );


        public static Object GetDataObject( DependencyObject owner )
        {
            return ( Object )owner.GetValue( DataObjectProperty );
        }

        public static void SetDataObject( DependencyObject owner, Object value )
        {
            owner.SetValue( DataObjectProperty, value );
        }

        #endregion

        private static void OnDataObjectChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var attached = DragDropManager.GetIsDragSourceAttached( d );
            if( !attached )
            {
                DragDropManager.SetIsDragSourceAttached( d, true );

                ( ( UIElement )d ).PreviewMouseLeftButtonDown += ( s, args ) =>
                {
                    var _startPoint = args.GetPosition( null );
                    DragDropManager.SetStartPoint( d, _startPoint );
                };

                /*
                 * We cannot use "MouseLeftButtonDown" because on the
                 * TreeView control is never fired, maybe is handled by
                 * someone else in the processing pipeline.
                 */
                //( ( UIElement )d ).MouseLeftButtonDown += ( s, args ) =>
                //{
                //    var _startPoint = args.GetPosition( null );
                //    DragDropManager.SetStartPoint( d, _startPoint );
                //};

                ( ( UIElement )d ).MouseMove += ( s, args ) =>
                {
                    var isDragging = DragDropManager.GetIsDragging( d );
                    if( args.LeftButton == MouseButtonState.Pressed && !isDragging )
                    {
                        var position = args.GetPosition( null );
                        var _startPoint = DragDropManager.GetStartPoint( d );

                        if( Math.Abs( position.X - _startPoint.X ) > SystemParameters.MinimumHorizontalDragDistance ||
                            Math.Abs( position.Y - _startPoint.Y ) > SystemParameters.MinimumVerticalDragDistance )
                        {
                            StartDrag( d, args );
                        }
                    }
                };
            }
        }

        static void StartDrag( DependencyObject d, MouseEventArgs e )
        {
            var sourceItem = DragDropManager.FindDragContainer( ( DependencyObject )e.OriginalSource );

            DragDropManager.SetIsDragging( d, true );

            var obj = DragDropManager.GetDataObject( sourceItem );
            var objType = DragDropManager.GetDataObjectType( sourceItem );

            DataObject data = null;
            if( String.IsNullOrEmpty( objType ) )
            {
                data = new DataObject( obj.GetType(), obj );
            }
            else
            {
                data = new DataObject( objType, obj );
            }

            var de = DragDrop.DoDragDrop( d, data, DragDropEffects.Move );

            DragDropManager.SetIsDragging( d, false );
        }

        #region Attached Property: IsDragging

        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.RegisterAttached(
                                      "IsDragging",
                                      typeof( Boolean ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( false ) );


        /// <summary>
        /// Gets the is dragging.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        public static Boolean GetIsDragging( DependencyObject owner )
        {
            return ( Boolean )owner.GetValue( IsDraggingProperty );
        }

        static void SetIsDragging( DependencyObject owner, Boolean value )
        {
            owner.SetValue( IsDraggingProperty, value );
        }

        #endregion

        #region Attached Property: StartPoint

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.RegisterAttached(
                                      "StartPoint",
                                      typeof( Point ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null ) );


        static Point GetStartPoint( DependencyObject owner )
        {
            return ( Point )owner.GetValue( StartPointProperty );
        }

        static void SetStartPoint( DependencyObject owner, Point value )
        {
            owner.SetValue( StartPointProperty, value );
        }

        #endregion

        #region Attached Property: OnDropCommand

        public static readonly DependencyProperty OnDropCommandProperty = DependencyProperty.RegisterAttached(
                                      "OnDropCommand",
                                      typeof( ICommand ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null, OnOnDropCommandChanged ) );


        public static ICommand GetOnDropCommand( DependencyObject owner )
        {
            return ( ICommand )owner.GetValue( OnDropCommandProperty );
        }

        public static void SetOnDropCommand( DependencyObject owner, ICommand value )
        {
            owner.SetValue( OnDropCommandProperty, value );
        }

        #endregion

        #region Attached Property: OnDragEnterCommand

        public static readonly DependencyProperty OnDragEnterCommandProperty = DependencyProperty.RegisterAttached(
                                      "OnDragEnterCommand",
                                      typeof( ICommand ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null ) );


        public static ICommand GetOnDragEnterCommand( DependencyObject owner )
        {
            return ( ICommand )owner.GetValue( OnDragEnterCommandProperty );
        }

        public static void SetOnDragEnterCommand( DependencyObject owner, ICommand value )
        {
            owner.SetValue( OnDragEnterCommandProperty, value );
        }

        #endregion

        #region Attached Property: OnDragLeaveCommand

        public static readonly DependencyProperty OnDragLeaveCommandProperty = DependencyProperty.RegisterAttached(
                                      "OnDragLeaveCommand",
                                      typeof( ICommand ),
                                      typeof( DragDropManager ),
                                      new FrameworkPropertyMetadata( null ) );


        public static ICommand GetOnDragLeaveCommand( DependencyObject owner )
        {
            return ( ICommand )owner.GetValue( OnDragLeaveCommandProperty );
        }

        public static void SetOnDragLeaveCommand( DependencyObject owner, ICommand value )
        {
            owner.SetValue( OnDragLeaveCommandProperty, value );
        }

        #endregion

        /// <summary>
        /// Finds the drag container that is the WPF element 
        /// that holds the DataObject that will be dragged.
        /// </summary>
        /// <param name="originalSource">
        /// The original source where the 
        /// drag operation started.
        /// </param>
        /// <returns>
        /// The element that holds the DataObject, 
        /// or null if none can be found.
        /// </returns>
        static DependencyObject FindDragContainer( DependencyObject originalSource )
        {
            var element = originalSource.FindParent<DependencyObject>( t =>
            {
                return DragDropManager.GetDataObject( t ) != null;
            } );

            return element;
        }

        /// <summary>
        /// Finds the WPF element where the DropTarget property has been defined.
        /// </summary>
        /// <param name="originalSource">
        /// The original source where the Drop, 
        /// or DragOver, operation is happening.
        /// </param>
        /// <returns>
        /// The element that holds the DropTarget, 
        /// or null if none can be found.
        /// </returns>
        static DependencyObject FindDropTargetContainer( DependencyObject originalSource )
        {
            var element = originalSource.FindParent<DependencyObject>( t =>
            {
                return DragDropManager.GetDropTarget( t ) != null;
            } );

            return element;
        }

        static Object FindDropTarget( DependencyObject originalSource )
        {
            var element = DragDropManager.FindDropTargetContainer( originalSource );
            if( element != null )
            {
                return DragDropManager.GetDropTarget( element );
            }

            return null;
        }

        /// <summary>
        /// Finds the WPF element where the drop command is attached.
        /// </summary>
        /// <param name="originalSource">
        /// The original source where the Drop, 
        /// or DragOver, operation is happening.
        /// </param>
        /// <returns>
        /// The element that holds the drop command, 
        /// or null if none can be found.
        /// </returns>
        static DependencyObject FindDropCommandHolder( DependencyObject originalSource )
        {
            var element = originalSource.FindParent<DependencyObject>( t =>
            {
                return DragDropManager.GetOnDropCommand( t ) != null;
            } );

            return element;
        }

        static ICommand FindDropCommand( DependencyObject originalSource )
        {
            var element = DragDropManager.FindDropCommandHolder( originalSource );
            if( element != null )
            {
                return DragDropManager.GetOnDropCommand( element );
            }

            return null;
        }

        static DependencyObject FindDragEnterCommandHolder( DependencyObject originalSource )
        {
            var element = originalSource.FindParent<DependencyObject>( t =>
            {
                return DragDropManager.GetOnDragEnterCommand( t ) != null;
            } );

            return element;
        }

        static ICommand FindDragEnterCommand( DependencyObject originalSource )
        {
            var element = DragDropManager.FindDragEnterCommandHolder( originalSource );
            if( element != null )
            {
                return DragDropManager.GetOnDragEnterCommand( element );
            }

            return null;
        }

        static DependencyObject FindDragLeaveCommandHolder( DependencyObject originalSource )
        {
            var element = originalSource.FindParent<DependencyObject>( t =>
            {
                return DragDropManager.GetOnDragLeaveCommand( t ) != null;
            } );

            return element;
        }

        static ICommand FindDragLeaveCommand( DependencyObject originalSource )
        {
            var element = DragDropManager.FindDragLeaveCommandHolder( originalSource );
            if( element != null )
            {
                return DragDropManager.GetOnDragLeaveCommand( element );
            }

            return null;
        }

        private static void OnOnDropCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var attached = DragDropManager.GetIsDropTargetAttached( d );
            if( !attached )
            {
                DragDropManager.SetIsDropTargetAttached( d, true );

                var ui = ( UIElement )d;

                ui.AllowDrop = true;
                var ctrl = ui as Control;
                if( ctrl != null )
                {
                    var bkg = ctrl.GetValue( ItemsControl.BackgroundProperty );
                    if( bkg == null )
                    {
                        ctrl.SetValue( ItemsControl.BackgroundProperty, Brushes.Transparent );
                    }
                }

                ui.DragEnter += ( s, args ) =>
                {
                    var os = ( DependencyObject )args.OriginalSource;
                    var command = DragDropManager.FindDragEnterCommand( os );
                    if( command != null )
                    {
                        var dropTarget = DragDropManager.FindDropTarget( os );
                        var cmdArgs = new DragEnterArgs(
                            args.Data,
                            args.KeyStates,
                            dropTarget,
                            args.AllowedEffects );

                        if( command.CanExecute( cmdArgs ) )
                        {
                            command.Execute( cmdArgs );
                        }
                    }
                };

                ui.DragLeave += ( s, args ) =>
                {
                    var os = ( DependencyObject )args.OriginalSource;
                    var command = DragDropManager.FindDragLeaveCommand( os );
                    if( command != null )
                    {
                        var dropTarget = DragDropManager.FindDropTarget( os );
                        var cmdArgs = new DragLeaveArgs(
                            args.Data,
                            args.KeyStates,
                            dropTarget,
                            args.AllowedEffects );

                        if( command.CanExecute( cmdArgs ) )
                        {
                            command.Execute( cmdArgs );
                        }
                    }
                };

                ui.DragOver += ( s, args ) =>
                {
                    var os = ( DependencyObject )args.OriginalSource;

                    var command = DragDropManager.FindDropCommand( os );
                    if( command != null )
                    {
                        var dropTarget = DragDropManager.FindDropTarget( os );

                        Point position = new Point( 0, 0 );
                        if( os is IInputElement )
                        {
                            position = args.GetPosition( ( IInputElement )os );
                        }

                        var cmdArgs = new DragOverArgs(
                            args.Data,
                            args.KeyStates,
                            dropTarget,
                            args.AllowedEffects,
                            position );

                        var result = command.CanExecute( cmdArgs );
                        if( !result )
                        {
                            args.Effects = cmdArgs.Effects;
                            args.Handled = true;
                        }
                    }
                    else
                    {
                        args.Effects = args.AllowedEffects;
                        args.Handled = true;
                    }
                };

                ui.Drop += ( s, args ) =>
                {
                    var os = ( DependencyObject )args.OriginalSource;

                    var command = DragDropManager.FindDropCommand( os );
                    if( command != null )
                    {
                        var dropTarget = DragDropManager.FindDropTarget( os );

                        Point position = new Point( 0, 0 );
                        if( os is IInputElement )
                        {
                            position = args.GetPosition( ( IInputElement )os );
                        }

                        var cmdArgs = new DropArgs( args.Data, args.KeyStates, dropTarget, position );
                        command.Execute( cmdArgs );
                    }
                };
            }
        }
    }
}