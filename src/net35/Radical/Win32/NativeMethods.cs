using System;
using System.Runtime.InteropServices;

namespace Topics.Radical.Win32
{
#pragma warning disable 1591

	/// <summary>
	/// A bridge to frequently used
	/// OS APIs
	/// </summary>
	public static class NativeMethods
	{
#if COMPACT_FRAMEWORK 
                 private const string Kernel32 = "coredll.dll"; 
                 private const string User32 = "coredll.dll"; 
                 private const string Gdi32 = "coredll.dll"; 
#else
		private const string Kernel32 = "kernel32.dll";
		private const string User32 = "user32.dll";
		private const string Gdi32 = "gdi32.dll";
#endif 
 

		//[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		//public static extern IntPtr PostMessage( HandleRef hwnd, Int32 msg, Int32 wparam, Int32 lparam );

		//[DllImport( "User32.dll" )]
		//public static extern IntPtr SendMessage( IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam );

		////[DllImport( "shell32.dll" )]
		////internal static extern IntPtr SHGetFileInfo( String pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags );

		////[DllImport( "User32.dll" )]
		////internal static extern Int32 SetWindowsHookEx( Int32 idHook, Topics.Win32.Hooks.KeyboardHookDelegate lpfn, Int32 hMode, Int32 dwThreadId );

		//[DllImport( "User32.dll" )]
		//internal static extern Int32 UnhookWindowsHookEx( IntPtr hHook );

		//[DllImport( "User32.dll" )]
		//internal static extern Int32 GetAsyncKeyState( Int32 vKey );

		//[DllImport( "User32.dll" )]
		//internal static extern Int32 GetKeyState( Int32 vKey );

		//[DllImport( "User32.dll" )]
		//internal static extern int CallNextHookEx( IntPtr hhk, Int32 nCode, Int32 wParam, int lParam );
													
		////[DllImport( "shell32.dll" )]
		////internal static extern Boolean Shell_NotifyIcon( uint dwMessage, [In] ref NOTIFYICONDATA pnid );

		//[DllImport( "user32.dll", CharSet = CharSet.Auto, ExactSpelling = true )]
		//internal static extern Boolean TrackPopupMenuEx( HandleRef hmenu, Int32 fuFlags, Int32 x, Int32 y, HandleRef hwnd, IntPtr tpm );

		//[DllImport( "User32.Dll" )]
		//internal static extern System.Int32 GetCursorPos( ref POINT point );

		//[DllImport( "User32.Dll" )]
		//public static extern Boolean SetForegroundWindow( IntPtr hWnd );

		//[DllImport( "user32.dll" )]
		//public static extern bool ShowWindowAsync( IntPtr hWnd, Int32 nCmdShow );

		//[DllImport( "User32.dll" )]
		//internal static extern Boolean OpenIcon( IntPtr hWnd );

		//[DllImport( "User32.dll" )]
		//public static extern Boolean IsIconic( IntPtr hWnd );

		//[DllImport( "User32.dll" )]
		//internal static extern IntPtr SetFocus( IntPtr hWnd );

		//[DllImport( "user32.dll" )]
		//internal static extern Boolean SetWindowPlacement( IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl );

		//[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		//internal static extern uint ExtractIconEx( String szFileName, Int32 nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons );

		//[DllImport( "Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true )]
		//internal static extern int NetGetJoinInformation( [In, MarshalAs( UnmanagedType.LPWStr )] string server, out IntPtr domain, out NetJoinStatus status );

		//[DllImport( "Netapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		//internal static extern int DsGetDcName
		//(
		//    [MarshalAs( UnmanagedType.LPTStr )]
		//    String computerName,
		//    [MarshalAs( UnmanagedType.LPTStr )]
		//    String domainName,
		//    [In]Int32 domainGuid,
		//    [MarshalAs( UnmanagedType.LPTStr )]
		//    String siteName,
		//    UInt32 flags,
		//    out IntPtr DOMAIN_CONTROLLER_INFO
		//);

		//[DllImport( "Netapi32.dll", SetLastError = true )]
		//internal static extern int NetApiBufferFree( IntPtr Buffer );

		//[DllImport( "user32.dll" )]
		//internal static extern bool FlashWindowEx
		//( 
		//    [MarshalAs( UnmanagedType.Struct )]
		//    ref FLASHWINFO pfwi 
		//);

		[DllImport( User32, EntryPoint = "GetWindowLong" )]
		private static extern int Window_GetLong32(
				[In] IntPtr hWnd,
				[In][MarshalAs( UnmanagedType.U4 )] WindowLong index );

		[DllImport( User32, EntryPoint = "SetWindowLong" )]
		private static extern int Window_SetLong32(
				[In] IntPtr hWnd,
				[In][MarshalAs( UnmanagedType.U4 )] WindowLong index,
				[In] int value );

		[DllImport( User32, EntryPoint = "GetWindowLongPtrW" )]
		private static extern IntPtr Window_GetLong64(
				[In] IntPtr hWnd,
				[In][MarshalAs( UnmanagedType.U4 )] WindowLong index );

		[DllImport( User32, EntryPoint = "SetWindowLongPtrW" )]
		private static extern IntPtr Window_SetLong64(
				[In] IntPtr hWnd,
				[In][MarshalAs( UnmanagedType.U4 )] WindowLong index,
				[In] IntPtr value );

		public static IntPtr GetWindowLong(
				IntPtr hWnd,
				WindowLong index )
		{
			// Vista WoW64 does not implement GetWindowLong 
			if( IntPtr.Size == 4 )
			{
				return ( IntPtr )Window_GetLong32( hWnd, index );
			}
			else
			{
				return Window_GetLong64( hWnd, index );
			}
		}

		public static IntPtr SetWindowLong(
				IntPtr hWnd,
				WindowLong index,
				IntPtr value )
		{
			// Vista WoW64 does not implement SetWindowLong 
			if( IntPtr.Size == 4 )
			{
				return ( IntPtr )Window_SetLong32( hWnd, index, value.ToInt32() );
			}
			else
			{
				return Window_SetLong64( hWnd, index, value );
			}
		}
	}

#pragma warning restore 1591
}
