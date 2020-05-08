using System;
using System.IO;
using System.Reflection;

namespace RoboBrowser.CefSharpInitialization
{
    internal static class CefAssemblyPathResolver
    {
        private static string _assemblyDirectory;

        private static string AssemblyDirectory
        {
            get
            {
                if (_assemblyDirectory is null)
                {
                    var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    var uri = new UriBuilder(codeBase);
                    var path = Uri.UnescapeDataString(uri.Path);
                    _assemblyDirectory = Path.GetDirectoryName(path);
                }

                return _assemblyDirectory;
            }
        }

        public static string GetAssemblyPath(string assemblyName) => Path.Combine(
            AssemblyDirectory,
            Environment.Is64BitProcess ? "x64" : "x86",
            assemblyName);
    }
}
