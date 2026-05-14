using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Services
{
    internal class BiliApiService
    {
        private BiliCoreClient? _biliCoreClient;
        private HttpClient? _httpClient;
        private static readonly string defaultKey = typeof(Main).Assembly.FullName!;
        private readonly IApiFactory apiFactory;

        public BiliCoreClient BiliClient { 
            get {
                if(_biliCoreClient is not null)
                    return _biliCoreClient;

                _httpClient?.Dispose();
                _httpClient = null;
                _httpClient = apiFactory.GetClient(defaultKey);
                _biliCoreClient ??= new BiliCoreClient(_httpClient);
                return _biliCoreClient;
            } 
        }

        public BiliApiService(IApiFactory apiFactory)
        {
            this.apiFactory = apiFactory;
            apiFactory.Configure(defaultKey, HttpConfigure);
            apiFactory.ProxyChanged += ApiFactory_ProxyChanged;
        }

        private void HttpConfigure(HttpClient httpClient, HttpClientHandler httpClientHandler)
        {
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://www.bilibili.com");
        }

        public void Dispose()
        {
            apiFactory.ProxyChanged -= ApiFactory_ProxyChanged;
            apiFactory.Remove(defaultKey);
        }

        private void ApiFactory_ProxyChanged()
        {
            _biliCoreClient = null;
        }
    }
}
