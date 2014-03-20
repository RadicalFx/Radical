//using System;
//using Microsoft.Phone.Shell;

//namespace Topics.Radical.Windows.Controls
//{
//	/// <summary>
//	/// A IconButton for the BindableApplicationBar.
//	/// </summary>
//	public class BindableApplicationBarIconButton :
//		BindableApplicationBarElement,
//		IApplicationBarIconButton
//	{
//		internal ApplicationBarIconButton GetButton()
//		{
//			return ( ApplicationBarIconButton )this.wrappedItem;
//		}

//		/// <summary>
//		/// Initializes a new instance of the <see cref="BindableApplicationBarIconButton"/> class.
//		/// </summary>
//		public BindableApplicationBarIconButton()
//			: base( new ApplicationBarIconButton() )
//		{

//		}

//		/// <summary>
//		/// The URI of the icon to use for the button.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Uri"/>.</returns>
//		public Uri IconUri
//		{
//			get { return ( ( IApplicationBarIconButton )base.wrappedItem ).IconUri; }
//			set { ( ( IApplicationBarIconButton )base.wrappedItem ).IconUri = value; }
//		}
//	}
//}