global using Newtonsoft.Json;
global using System.Runtime.CompilerServices;
using System.Reflection;

[assembly: InternalsVisibleTo("ApiClient.test")]

[assembly: AssemblyProduct("Massive Api Client")]
[assembly: AssemblyVersion("0.2.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif