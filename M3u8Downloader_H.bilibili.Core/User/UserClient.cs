using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace M3u8Downloader_H.bilibili.Core.User
{
    public class UserClient(HttpClient httpClient)
    {
        //失败"code":-101,"message":"账号未登录"
        public async  Task<UserInfo> GetUserInfo(CancellationToken cancellationToken = default)
        {
            var resp = await httpClient.SendHttpRequestAsync("https://api.bilibili.com/x/web-interface/nav", cancellationToken);
            var user = JsonSerializer.Deserialize(resp, UserContext.Default.User)
                        ?? throw new InvalidDataException("获取用户信息失败");
            if (user.Code != 0)
                throw new InvalidDataException(user.Message);

            return user.Data;
        }
    }
}
