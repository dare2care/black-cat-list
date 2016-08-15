using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Owin;

[assembly: AssemblyTitle("BlackCatList.Web")]
[assembly: AssemblyCompany("Dare 2 Care Animals")]
[assembly: AssemblyProduct("Black Cat List")]
[assembly: AssemblyCopyright("Copyright © Dare 2 Care Animals 2016")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("d1ea68b7-6ea4-496b-98d6-3176208ded02")]

[assembly: AssemblyVersion("1.0.*")]

[assembly: OwinStartupAttribute(typeof(BlackCatList.Web.Startup))]
