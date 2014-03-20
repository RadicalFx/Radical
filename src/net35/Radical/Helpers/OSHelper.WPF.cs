using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Helpers
{
	public class OSHelper
	{
		public Int32 GetOSArchitecture()
		{
			string pa = Environment.GetEnvironmentVariable( "PROCESSOR_ARCHITECTURE" );
			return ( ( String.IsNullOrEmpty( pa ) || String.Compare( pa, 0, "x86", 0, 3, true ) == 0 ) ? 32 : 64 );
		}

		public string GetOSInfo()
		{
			//Get Operating system information.
			OperatingSystem os = Environment.OSVersion;
			//Get version information about the os.
			Version vs = os.Version;

			//Variable to hold our return value
			string operatingSystem = "";

			if( os.Platform == PlatformID.Win32Windows )
			{
				//This is a pre-NT version of Windows
				switch( vs.Minor )
				{
					case 0:
						operatingSystem = "95";
						break;
					case 10:
						if( vs.Revision.ToString() == "2222A" )
							operatingSystem = "98SE";
						else
							operatingSystem = "98";
						break;
					case 90:
						operatingSystem = "Me";
						break;
					default:
						break;
				}
			}
			else if( os.Platform == PlatformID.Win32NT )
			{
				switch( vs.Major )
				{
					case 3:
						operatingSystem = "NT 3.51";
						break;
					case 4:
						operatingSystem = "NT 4.0";
						break;
					case 5:
						if( vs.Minor == 0 )
							operatingSystem = "2000";
						else
							operatingSystem = "XP";
						break;
					case 6:
						if( vs.Minor == 0 )
							operatingSystem = "Vista";
						else
							operatingSystem = "7";
						break;
					default:
						break;
				}
			}
			//Make sure we actually got something in our OS check
			//We don't want to just return " Service Pack 2" or " 32-bit"
			//That information is useless without the OS version.
			if( operatingSystem != "" )
			{
				//Got something.  Let's prepend "Windows" and get more info.
				operatingSystem = "Windows " + operatingSystem;
				//See if there's a service pack installed.
				if( os.ServicePack != "" )
				{
					//Append it to the OS name.  i.e. "Windows XP Service Pack 3"
					operatingSystem += " " + os.ServicePack;
				}
				//Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
				operatingSystem += " " + GetOSArchitecture().ToString() + "-bit";
			}
			//Return the information we've gathered.
			return operatingSystem;
		}
	}
}
