using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using Topics.Radical.Win32;
using System.Windows.Interop;

namespace Topics.Radical.Windows.Behaviors
{
	public sealed class WindowControlBoxBehavior : Behavior<Window>
	{
		//#region Dependency Property: AllowMaximize

		//public static readonly DependencyProperty AllowMaximizeProperty = DependencyProperty.Register(
		//    "AllowMaximize",
		//    typeof( Boolean ),
		//    typeof( WindowControlBoxBehavior ),
		//    new PropertyMetadata( true ) );

		//public Boolean AllowMaximize
		//{
		//    get { return ( Boolean )this.GetValue( AllowMaximizeProperty ); }
		//    set { this.SetValue( AllowMaximizeProperty, value ); }
		//}

		//#endregion

		public Boolean AllowMaximize
		{
			get;
			set;
		}

		//#region Dependency Property: AllowMinimize

		//public static readonly DependencyProperty AllowMinimizeProperty = DependencyProperty.Register(
		//    "AllowMinimize",
		//    typeof( Boolean ),
		//    typeof( WindowControlBoxBehavior ),
		//    new PropertyMetadata( true ) );

		//public Boolean AllowMinimize
		//{
		//    get { return ( Boolean )this.GetValue( AllowMinimizeProperty ); }
		//    set { this.SetValue( AllowMinimizeProperty, value ); }
		//}

		//#endregion

		public Boolean AllowMinimize
		{
			get;
			set;
		}

		EventHandler h;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowControlBoxBehavior"/> class.
		/// </summary>
		public WindowControlBoxBehavior()
		{
			h = ( s, e ) =>
			{
				var isDesign = DesignTimeHelper.GetIsInDesignMode();
				var hWnd = new WindowInteropHelper( this.AssociatedObject ).Handle;

				if( !isDesign && hWnd != IntPtr.Zero && ( !this.AllowMaximize || !this.AllowMinimize ) )
				{
					var windowLong = NativeMethods.GetWindowLong( hWnd, WindowLong.Style ).ToInt32();

					if( !this.AllowMaximize )
					{
						windowLong = windowLong & ~Constants.WS_MAXIMIZEBOX;
					}

					if( !this.AllowMinimize )
					{
						windowLong = windowLong & ~Constants.WS_MINIMIZEBOX;
					}

					NativeMethods.SetWindowLong( hWnd, WindowLong.Style, ( IntPtr )windowLong );
				}
			};
		}

		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.SourceInitialized += h;
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			this.AssociatedObject.SourceInitialized -= h;
			base.OnDetaching();
		}
	}
}
