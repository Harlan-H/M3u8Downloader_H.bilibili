using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace M3u8Downloader_H.bilibili.Core.Streams
{
    public class StreamId(VideoId videoId, long aid, PlayList playList)
    {
        private static string Session => System.Guid.NewGuid().ToString("N");

        public string PlayUrl =>
            $"https://api.bilibili.com/x/player/wbi/playurl?avid={aid}&bvid={videoId}&cid={playList.Cid}&qn=112&fnver=0&type=&otype=json&fourk=1&fnval=4048&gaia_source=&from_client=BROWSER&is_main_page=true&need_fragment=false&isGaiaAvoided=false&session={Session}&voice_balance=1&web_location=1315873&w_rid={Session}&wts={DateTimeOffset.Now.ToUnixTimeSeconds()}";

        public string CloseCaptionUrl =>
            $"https://api.bilibili.com/x/player/wbi/v2?aid={aid}&cid={playList.Cid}";
    }
}
