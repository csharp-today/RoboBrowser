using RoboBrowser.CefSharpInitialization;
using System.Threading.Tasks;

namespace RoboBrowser
{
    public class BrowserFactory
    {
        public bool Verbose { get; set; }

        public async Task<IBrowser> GetBrowserAsync()
        {
            await CefSharpGlobalInitializer.InitializeCef();

            var browser = new Browser(Verbose);
            await browser.InitializeAsync();

            return browser;
        }

        public Task ShutdownAsync() =>
            CefSharpGlobalInitializer.ShutdownCef();
    }
}
