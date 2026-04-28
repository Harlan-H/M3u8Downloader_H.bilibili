using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Core.Extensions
{
    public static class HttpClientExtensions
    {
        extension(HttpClient httpClient)
        {
            public async ValueTask<string> SendHttpRequestAsync(
                  HttpRequestMessage httpRequestMessage,
                  CancellationToken cancellationToken = default)
            {
                using var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"请求失败 错误码是:{response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }

            public async ValueTask<string> SendHttpRequestAsync(
                string url,
                CancellationToken cancellationToken = default
                )
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!request.Headers.Contains("referer"))
                {
                    request.Headers.Add("referer", "https://www.bilibili.com");
                }
                return await httpClient.SendHttpRequestAsync(request, cancellationToken);
            }
        }
    }
}
