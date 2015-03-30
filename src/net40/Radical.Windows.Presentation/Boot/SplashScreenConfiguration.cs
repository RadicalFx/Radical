using System;
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
			this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			this.SizeToContent = SizeToContent.WidthAndHeight;
			this.WindowStyle = WindowStyle.None;
			this.MinimumDelay = 1500;
			this.SplashScreenViewType = typeof( SplashScreenView );

			this.StartupAsyncWork = obj => System.Threading.Thread.Sleep( this.MinimumDelay );
		}

		/// <summary>
		/// 
		/// </summary>
		public SizeToContent SizeToContent { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public WindowStartupLocation WindowStartupLocation { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public WindowStyle WindowStyle { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Action<IServiceProvider> StartupAsyncWork { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Double Height { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Double Width { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32 MinimumDelay { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Type SplashScreenViewType { get; set; }
	}
}
