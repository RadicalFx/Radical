using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Windows;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "Radical.Windows" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCulture( "" )]

//This is the real build number used for references
[assembly: AssemblyVersion( "1.3.16.0" )]

//This build number should always be equal to the used AssemblyVersion, currently is not used.
[assembly: AssemblyFileVersion( "1.3.16.0" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

#if !SILVERLIGHT

// Specifies the location in which theme dictionaries are stored for types in an assembly.
[assembly: ThemeInfo(
	// Specifies the location of system theme-specific resource dictionaries for this project.
	// The default setting in this project is "None" since this default project does not
	// include these user-defined theme files:
	//     Themes\Aero.NormalColor.xaml
	//     Themes\Classic.xaml
	//     Themes\Luna.Homestead.xaml
	//     Themes\Luna.Metallic.xaml
	//     Themes\Luna.NormalColor.xaml
	//     Themes\Royale.NormalColor.xaml
	ResourceDictionaryLocation.None,

	// Specifies the location of the system non-theme specific resource dictionary:
	//     Themes\generic.xaml
	ResourceDictionaryLocation.SourceAssembly )]

#endif