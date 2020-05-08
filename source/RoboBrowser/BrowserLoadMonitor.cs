using CefSharp;
using CefSharp.OffScreen;
using System.Threading.Tasks;

namespace RoboBrowser
{
    internal class BrowserLoadMonitor
    {
        private readonly ChromiumWebBrowser _browser;
        private readonly TaskCompletionSource<bool> _taskCompletitionSource = new TaskCompletionSource<bool>();

        public Task LoadedTask => _taskCompletitionSource.Task;

        public BrowserLoadMonitor(ChromiumWebBrowser browser)
        {
            _browser = browser;
            _browser.LoadingStateChanged += LoadingStateChanged;
        }

        private void LoadingStateChanged(object sender, LoadingStateChangedEventArgs ea)
        {
            if (!ea.IsLoading)
            {
                _browser.LoadingStateChanged -= LoadingStateChanged;
                _taskCompletitionSource.SetResult(true);
            }
        }
    }
}
