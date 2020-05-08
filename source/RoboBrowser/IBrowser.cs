using System;
using System.Threading.Tasks;

namespace RoboBrowser
{
    public interface IBrowser : IDisposable
    {
        string Address { get; }

        Task ClickButtonAsync(ElementSelector button, bool waitForLoad = true);
        Task ClickEnabledButtonAsync(ElementSelector button, bool waitForLoad = true);
        Task<string> DownloadAsync(string url);
        Task<string> GetSourceAsTextAsync();
        Task<object> GetValueAsync(ElementSelector element);
        Task LoadAsync(string url, bool waitForLoad = true);
        Task SetValueAsync(ElementSelector element, string value, string valueProperty = null);
    }
}
