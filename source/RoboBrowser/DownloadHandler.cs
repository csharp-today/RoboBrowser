using CefSharp;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboBrowser
{
    internal class DownloadHandler : IDownloadHandler
    {
        private static readonly Regex _nameRegex = new Regex(@"filename=""(.*)""");
        private readonly TaskCompletionSource<string> _taskCompletionSource = new TaskCompletionSource<string>();
        private int _lastProgress = 0;

        public Task<string> DownloadedTask => _taskCompletionSource.Task;

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!string.IsNullOrWhiteSpace(downloadItem.SuggestedFileName))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), downloadItem.SuggestedFileName);
                callback.Continue(path, false);
                return;
            }

            var match = _nameRegex.Match(downloadItem.ContentDisposition);
            if (match.Success)
            {
                var name = match.Groups[1].ToString();
                var path = Path.Combine(Directory.GetCurrentDirectory(), name);
                callback.Continue(path, false);
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, CefSharp.IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            if (_lastProgress != downloadItem.PercentComplete)
            {
                _lastProgress = downloadItem.PercentComplete;
                Console.WriteLine($"Download progress: {downloadItem.PercentComplete}%");
            }
            if (downloadItem.IsComplete)
            {
                _taskCompletionSource.SetResult(downloadItem.FullPath);
            }
        }
    }
}
