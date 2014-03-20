using System;
using System.Windows.Threading;
using System.Windows;

namespace Topics.Radical.Windows
{
	public class RadicalWpfApplication : Application
	{
		static readonly DispatcherOperationCallback exitFrameCallback = new DispatcherOperationCallback( ExitFrame );

		/// <summary>
		/// Processes all UI messages currently in the message queue.
		/// </summary>
		public static void DoEvents()
		{
			// Create new nested message pump.
			var nestedFrame = new DispatcherFrame();

			// Dispatch a callback to the current message queue, when getting called, 
			// this callback will end the nested message loop.
			// note that the priority of this callback should be lower than the that of UI event messages.
			var exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke( DispatcherPriority.Background, exitFrameCallback, nestedFrame );

			// pump the nested message loop, the nested message loop will 
			// immediately process the messages left inside the message queue.
			Dispatcher.PushFrame( nestedFrame );

			// If the "exitFrame" callback doesn't get finished, Abort it.
			if( exitOperation.Status != DispatcherOperationStatus.Completed )
			{
				exitOperation.Abort();
			}
		}

		private static Object ExitFrame( Object state )
		{
			DispatcherFrame frame = state as DispatcherFrame;

			// Exit the nested message loop.
			frame.Continue = false;

			return null;
		}
	}
}
