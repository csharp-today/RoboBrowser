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
                    var location = Assembly.GetExecutingAssembly().Location;
                    _assemblyDirectory = Path.GetDirectoryName(location);
                }

                return _assemblyDirectory;
            }
        }

        public static string GetAssemblyPath(string assemblyName) => Path.Combine(
            AssemblyDirectory,
            GetRoot(Environment.Is64BitProcess ? "x64" : "x86"),
            assemblyName);

        private static string GetRoot(string arch) =>
            $@"runtimes\win-{arch}\native";
    }
}