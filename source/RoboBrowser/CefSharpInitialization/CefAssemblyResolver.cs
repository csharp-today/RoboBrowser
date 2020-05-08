using System;
using System.IO;
using System.Reflection;

namespace RoboBrowser.CefSharpInitialization
{
    internal static class CefAssemblyResolver
    {
        public static void AttachCefAssemblyResolver(this AppDomain appDomain) =>
            appDomain.AssemblyResolve += AssemblyResolve;

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Source: https://github.com/cefsharp/CefSharp/issues/1714
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = CefAssemblyPathResolver.GetAssemblyPath(assemblyName);
                return File.Exists(archSpecificPath)
                    ? Assembly.LoadFile(archSpecificPath)
                    : null;
            }

            return null;
        }
    }
}
