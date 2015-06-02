using System;
using System.Threading;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.Boot
{
	/// <summary>
	/// The configuration of the SplashScreen.
	/// </summary>
	public class SplashScreenConfiguration
	{
		/// <summary>
		/// SplashScreenConfiguration default constructor.
		/// </summary>
		public SplashScreenConfiguration()
		{
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			this.SizeToContent = SizeToContent.WidthAndHeight;
			this.MinWidth = 485;
			this.MinHeight = 335;
			this.WindowStyle = WindowStyle.None;
			this.MinimumDelay = 1500;
			this.SplashScreenViewType = typeof( SplashScreenView );
#if FX40
			this.StartupAsyncWork = obj => Thread.Sleep( this.MinimumDelay );
#else
			this.StartupAsyncWork = obj => Task.Delay( this.MinimumDelay );
#endif
		}

		/// <summary>
		/// Determines the way the splash screen hosting window is dimensioned, the default value is <c>WidthAndHeight</c>.
		/// </summary>
		public SizeToContent SizeToContent { get; set; }
		
		/// <summary>
		/// The splash screen startup location, the default value is <c>CenterScreen</c>.
		/// </summary>
		public WindowStartupLocation WindowStartupLocation { get; set; }
		
		/// <summary>
		/// The splash screen window style, the default valkue is <c>None</c>.
		/// </summary>
		public WindowStyle WindowStyle { get; set; }

		/// <summary>
		/// Defines the work that shopuld be executed asynchronously while the splash screen is running.
		/// </summary>
		public Action<IServiceProvider> StartupAsyncWork { get; set; }

		/// <summary>
		/// Defines the Height of the splash screen window if the SizeToContent value is Manual or Widht; otherwise is ignored.
		/// </summary>
		public Double Height { get; set; }

		/// <summary>
		/// Defines the Width of the splash screen window if the SizeToContent value is Manual or Height; otherwise is ignored.
		/// </summary>
		public Double Width { get; set; }

		/// <summary>
		/// Represents the minimum time, in milliseconds, the splash screen will be shown.
		/// </summary>
		public Int32 MinimumDelay { get; set; }

		/// <summary>
		/// Defines the default view that Radical use to host the splash screen content.
		/// </summary>
		public Type SplashScreenViewType { get; set; }

		/// <summary>
		/// The Minimum Width of the splash screen window. The default value is 585.
		/// </summary>
		public Double? MinWidth { get; set; }

		/// <summary>
		/// The Minimum Height of the splash screen window. The default value is 335.
		/// </summary>
		public Double? MinHeight { get; set; }
	}
}
