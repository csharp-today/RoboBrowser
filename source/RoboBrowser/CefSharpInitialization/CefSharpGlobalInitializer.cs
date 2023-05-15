using System.Threading;
using System.Threading.Tasks;

namespace RoboBrowser.CefSharpInitialization
{
    internal static class CefSharpGlobalInitializer
    {
        private static readonly CefSharpInitializer _cefSharpInitializer = new CefSharpInitializer();
        private static bool _isInitialized = false;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public static async Task InitializeCef()
        {
            if (_isInitialized) return;

            await _semaphore.WaitAsync();

            try
            {
                if (!_isInitialized)
                {
                    await _cefSharpInitializer.InitializeCef();
                    _isInitialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public static async Task ShutdownCef()
        {
            if (!_isInitialized) return;

            await _semaphore.WaitAsync();

            try
            {
                if (_isInitialized)
                {
                    await _cefSharpInitializer.ShutdownCef();
                    _isInitialized = false;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}