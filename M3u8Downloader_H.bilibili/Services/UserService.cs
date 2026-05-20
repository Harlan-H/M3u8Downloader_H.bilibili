using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Services
{
    public class UserService(BiliApiService biliApiService)
    {
        public async Task<string> GetUserInfoAsync(string cookie, CancellationToken cancellationToken = default)
        {
            biliApiService.SetCookie(cookie);
            var resp = await biliApiService.BiliClient.Users.GetUserInfo(cancellationToken);
            return resp.UserName;
        }
    }
}
