using M3u8Downloader_H.Abstractions.Models;

namespace M3u8Downloader_H.bilibili.Utils
{
    internal class ApiHttp<T> :IDisposable
        where T : class
    {
        private T? _api;
        private readonly IApiFactory apiFactory;
        private readonly string baseUrl;

        public T API  => _api ??= apiFactory.Create<T>(baseUrl, null);

        public ApiHttp(IApiFactory apiFactory, string baseUrl)
        {
            this.apiFactory = apiFactory;
            this.baseUrl = baseUrl;
            apiFactory.ProxyChanged += ApiFactory_ProxyChanged;
        }

        private void ApiFactory_ProxyChanged()
        {
            _api = apiFactory.Create<T>(baseUrl, null);
        }

        public void Dispose()
        {
            apiFactory.ProxyChanged -= ApiFactory_ProxyChanged;
            GC.SuppressFinalize(this);
        }
    }
}

