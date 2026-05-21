using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core;
using System.Net;

namespace M3u8Downloader_H.bilibili.Services
{
    public class BiliApiService
    {
        private BiliCoreClient? _biliCoreClient;
        private HttpClient? _httpClient;
        private CookieContainer? _cookieContainer;
        private static readonly string defaultKey = typeof(Main).Assembly.FullName!;
        private readonly IHttpFactory httpFactory;
        private readonly ICacheService cacheService;

        public BiliCoreClient BiliClient { 
            get {
                if(_biliCoreClient is not null)
                    return _biliCoreClient;

                _httpClient = httpFactory.GetClient(defaultKey);
                _biliCoreClient = new BiliCoreClient(_httpClient, cacheService);
                return _biliCoreClient;
            }
        }

        public HttpClient Client => _httpClient!;


        public BiliApiService(IHttpFactory httpFactory,ICacheService cacheService)
        {
            this.httpFactory = httpFactory;
            this.cacheService = cacheService;
            httpFactory.Configure(defaultKey, HttpConfigure);
            httpFactory.ProxyChanged += ApiFactory_ProxyChanged;
        }

        public void SetCookie(string cookie)
        {
            bool isSet = false;
            foreach (var part in cookie.Split(';',StringSplitOptions.TrimEntries))
            {
                if (part.StartsWith("SESSDATA"))
                {
                    _cookieContainer = new CookieContainer();
                    _cookieContainer.SetCookies(new Uri("https://api.bilibili.com"), part);
                    _cookieContainer.SetCookies(new Uri("https://www.bilibili.com"), part);
                    ResetClient();
                    isSet = true;
                    break;
                }
            }
            if (isSet == false)
                throw new InvalidDataException("cookie无效,没有【SESSDATA】关键字段");
        }

        private void HttpConfigure(HttpClient httpClient, HttpClientHandler httpClientHandler)
        {
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.bilibili.com");
            if (_cookieContainer is not null)
            {
                httpClientHandler.CookieContainer = _cookieContainer;
                httpClientHandler.UseCookies = true;
            }
        }

        public void Dispose()
        {
            httpFactory.ProxyChanged -= ApiFactory_ProxyChanged;
            httpFactory.Remove(defaultKey);
        }

        private void ResetClient()
        {
            httpFactory.CloseClient(defaultKey);
            _httpClient = null;
            _biliCoreClient = null;
        }

        private void ApiFactory_ProxyChanged()
        {
            ResetClient();
        }
    }
}
