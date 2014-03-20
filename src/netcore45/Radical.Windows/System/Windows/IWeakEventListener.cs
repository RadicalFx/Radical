﻿namespace System.Windows
{

	/// <summary>
	/// Provides event listening support for classes that expect to receive events  
	/// through the WeakEvent pattern and a WeakEventManager.  
	/// </summary>  
	public interface IWeakEventListener
	{
		/// <summary>  
		/// Receives events from the centralized event manager.  
		/// </summary>  
		/// <param name="managerType">The type of the WeakEventManager calling this method.</param>  
		/// <param name="sender">Object that originated the event.</param>  
		/// <param name="e">Event data.</param>  
		/// <returns>true if the listener handled the event. It is considered an error by the  
		/// WeakEventManager handling in WPF to register a listener for an event that the  
		/// listener does not handle. Regardless, the method should return false if it receives  
		/// an event that it does not recognize or handle.  
		/// </returns>  
		Boolean ReceiveWeakEvent( Type managerType, object sender, Object e );
	}
}
