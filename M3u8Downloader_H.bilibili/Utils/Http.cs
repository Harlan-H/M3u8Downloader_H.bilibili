using M3u8Downloader_H.Abstractions.Models;

namespace M3u8Downloader_H.bilibili.Utils
{
    internal class Http : IDisposable
    {
        private HttpClient? _httpClient;
        private static readonly string defaultKey = typeof(Main).Assembly.FullName!;
        private readonly IApiFactory apiFactory;

        public HttpClient Client => _httpClient ??= apiFactory.GetClient(defaultKey);

        public Http(IApiFactory apiFactory)
        {
            this.apiFactory = apiFactory;
            apiFactory.Configure(defaultKey, HttpConfigure);
            apiFactory.ProxyChanged += ApiFactory_ProxyChanged;
        }

        private void HttpConfigure(HttpClient httpClient,HttpClientHandler httpClientHandler)
        {
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.bilibili.com");
        }

        public void Dispose()
        {
            apiFactory.Remove(defaultKey);
        }

        private void ApiFactory_ProxyChanged()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }
    }
}
