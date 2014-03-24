using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace Topics.Radical.Diagnostics
{
	public class BindingErrorTraceListener : DefaultTraceListener
	{
		private static BindingErrorTraceListener _listener;

		/// <summary>
		/// Initializes the trace listener only if there is a debugger attached.
		/// </summary>
		public static void Initialize()
		{
			if( Debugger.IsAttached )
			{
				Initialize( SourceLevels.Error, TraceOptions.None );
			}
		}

		/// <summary>
		/// Initializes the trace listener only if there is a debugger attached.
		/// </summary>
		/// <param name="level">The level.</param>
		/// <param name="options">The options.</param>
		public static void Initialize( SourceLevels level, TraceOptions options )
		{
			if( Debugger.IsAttached )
			{
				if( _listener == null )
				{
					_listener = new BindingErrorTraceListener();
					PresentationTraceSources.DataBindingSource.Listeners.Add( _listener );
				}

				_listener.TraceOutputOptions = options;
				PresentationTraceSources.DataBindingSource.Switch.Level = level;
			}
		}

		/// <summary>
		/// Closes the trace.
		/// </summary>
		public static void CloseTrace()
		{
			if( _listener == null )
			{
				return;
			}

			_listener.Flush();
			_listener.Close();

			PresentationTraceSources.DataBindingSource.Listeners.Remove( _listener );

			_listener = null;
		}

		private StringBuilder _messageBuilder = new StringBuilder();

		/// <summary>
		/// Prevents a default instance of the <see cref="BindingErrorTraceListener"/> class from being created.
		/// </summary>
		private BindingErrorTraceListener()
		{

		}

		/// <summary>
		/// Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)"/> method.
		/// </summary>
		/// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)"/>.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/>
		///   </PermissionSet>
		public override void Write( string message )
		{
			_messageBuilder.Append( message );
		}

		/// <summary>
		/// Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)"/> method, followed by a carriage return and line feed (\r\n).
		/// </summary>
		/// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)"/>.</param>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/>
		///   </PermissionSet>
		public override void WriteLine( string message )
		{
			_messageBuilder.Append( message );

			var final = _messageBuilder.ToString();
			_messageBuilder.Length = 0;

			MessageBox.Show( final,
				"Binding Error",
				MessageBoxButton.OK,
				MessageBoxImage.Error );
		}
	}
}
