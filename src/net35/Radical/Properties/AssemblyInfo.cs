using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "Radical" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCulture( "" )]

[assembly: ComVisible( false )]
[assembly: Guid( "ac1104e2-8764-42ea-80aa-bfd22bc6a89f" )]

[assembly: CLSCompliant( true )]

//This is the real build number used for references
[assembly: AssemblyVersion( "1.5.2.1" )]

//This build number should always be equal to the one used in AssemblyVersion, currently is not used.
[assembly: AssemblyFileVersion( "1.5.2.1" )]

[assembly: NeutralResourcesLanguage( "en-US", UltimateResourceFallbackLocation.MainAssembly )]

//[assembly: InternalsVisibleTo( "Test.Radical, PublicKey=002400000480000094"
//                               + "0000000602000000240000525341310004000001"
//                               + "000100EBA94E6B3B1AC27D45D5F6F68D5DE3A935"
//                               + "DF183CCF3C11A9D0E2B23F62FE7B39374E03B2EF"
//                               + "1E83ECABD4C1072F7D2EAD06D3BDD5D95B2EEA02"
//                               + "E98BFF7739C89C4D3DE47F0927E447C1359A02E7"
//                               + "8D1515C18877785F7E755DDA1A33A2F6D2763A87"
//                               + "C758F8DDD622AA7C59E03C61D9A3C94D978DE834"
//                               + "7DEF4030016BCFBFE46AB2" )]
//[assembly: InternalsVisibleTo( "Test.Radical.Windows, PublicKey=002400000480000094"
//                               + "0000000602000000240000525341310004000001"
//                               + "000100EBA94E6B3B1AC27D45D5F6F68D5DE3A935"
//                               + "DF183CCF3C11A9D0E2B23F62FE7B39374E03B2EF"
//                               + "1E83ECABD4C1072F7D2EAD06D3BDD5D95B2EEA02"
//                               + "E98BFF7739C89C4D3DE47F0927E447C1359A02E7"
//                               + "8D1515C18877785F7E755DDA1A33A2F6D2763A87"
//                               + "C758F8DDD622AA7C59E03C61D9A3C94D978DE834"
//                               + "7DEF4030016BCFBFE46AB2" )]

[assembly: InternalsVisibleTo( "Test.Radical" )]
[assembly: InternalsVisibleTo( "Test.Radical.Windows" )]