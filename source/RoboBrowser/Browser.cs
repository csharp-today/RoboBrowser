using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Threading.Tasks;

namespace RoboBrowser
{
    internal class Browser : IBrowser
    {
        private readonly bool _verbose;
        private ChromiumWebBrowser _browser;

        public string Address => _browser.Address;

        internal Browser(bool verbose) => _verbose = verbose;

        public async Task ClickButtonAsync(ElementSelector button, bool waitForLoad = true)
        {
            var loadMonitor = waitForLoad ? new BrowserLoadMonitor(_browser) : null;
            await CallJavaScriptAsync(button + ".click();");

            if (loadMonitor != null)
            {
                await loadMonitor.LoadedTask;
            }
        }

        public async Task ClickEnabledButtonAsync(ElementSelector button, bool waitForLoad = true)
        {
            while (true)
            {
                var diabled = (bool)await GetValueAsync(button.Disabled);
                if (!diabled) break;
                await Task.Delay(100);
            }
            await ClickButtonAsync(button, waitForLoad);
        }

        public void Dispose() => _browser?.Dispose();

        public async Task<string> DownloadAsync(string url)
        {
            var handler = new DownloadHandler();
            _browser.DownloadHandler = handler;
            await LoadAsync(url, false);

            var localPath = await handler.DownloadedTask;
            _browser.DownloadHandler = null;
            return localPath;
        }

        public Task<string> GetSourceAsTextAsync() => _browser.GetTextAsync();

        public async Task<object> GetValueAsync(ElementSelector element) => await CallJavaScriptAsync(element + ";");

        public async Task InitializeAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            _browser = new ChromiumWebBrowser();
            _browser.BrowserInitialized += (_, __) => tcs.SetResult(true);
            await tcs.Task;
        }

        public async Task LoadAsync(string url, bool waitForLoad = true)
        {
            var loadMonitor = waitForLoad ? new BrowserLoadMonitor(_browser) : null;
            _browser.Load(url);

            if (loadMonitor != null)
            {
                await loadMonitor.LoadedTask;
            }
        }

        public Task SetValueAsync(ElementSelector element, string value, string valueProperty = null)
        {
            if (valueProperty is null)
            {
                valueProperty = ".value";
            }

            return CallJavaScriptAsync($"{element}{valueProperty} = `{value}`;");
        }

        private async Task<object> CallJavaScriptAsync(string script)
        {
            if (_verbose)
            {
                Console.WriteLine("JS: " + script);
            }

            var response = await _browser.EvaluateScriptAsync(script);
            if (!response.Success)
            {
                throw new ApplicationException("JavaScript call failed: " + response.Message);
            }

            return response.Result;
        }
    }
}
