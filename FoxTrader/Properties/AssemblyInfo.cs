using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Fox Trader")]
[assembly: AssemblyDescription("Fox styled macroeconomic simulator")]
[assembly: AssemblyConfiguration(
#if DEBUG
    "DEBUG"
#else
    "RELEASE"
#endif
)]
[assembly: AssemblyCompany("Fox Council")]
[assembly: AssemblyProduct("Fox Trader Game")]
[assembly: AssemblyCopyright("Copyright © FoxCouncil 2016-2025")]
[assembly: AssemblyTrademark("Fox Council")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("deaddead-beef-403e-bdb5-dcb778a8f8ab")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.1.*")]
[assembly: AssemblyFileVersion("0.1.0.0")]
[assembly: NeutralResourcesLanguage("en-US")]