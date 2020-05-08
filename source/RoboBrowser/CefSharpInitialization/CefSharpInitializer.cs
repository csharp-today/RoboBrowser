using CefSharp;
using CefSharp.OffScreen;
using System.Threading;
using System.Threading.Tasks;

namespace RoboBrowser.CefSharpInitialization
{
    internal class CefSharpInitializer
    {
        private TaskCompletionSource<bool> _cefShutdown = new TaskCompletionSource<bool>();
        private Thread _cefThread;

        public async Task InitializeCef()
        {
            var cefInitialize = new TaskCompletionSource<bool>();
            _cefThread = new Thread(new ThreadStart(() =>
            {
                // Set BrowserSubProcessPath based on app bitness at runtime
                var settings = new CefSettings();
                settings.BrowserSubprocessPath = CefAssemblyPathResolver.GetAssemblyPath("CefSharp.BrowserSubprocess.exe");

                // Make sure you set performDependencyCheck false
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

                cefInitialize.SetResult(true);

                while (!_cefShutdown.Task.IsCompleted)
                {
                    Thread.Yield();
                    Thread.Sleep(100);
                }
                _cefShutdown.Task.GetAwaiter().GetResult();

                Cef.Shutdown();
            }));

            _cefThread.Start();
            await cefInitialize.Task;
        }

        public async Task ShutdownCef()
        {
            _cefShutdown.SetResult(true);
            while (_cefThread.IsAlive)
            {
                await Task.Delay(100);
            }
            _cefThread.Join();
        }
    }
}
