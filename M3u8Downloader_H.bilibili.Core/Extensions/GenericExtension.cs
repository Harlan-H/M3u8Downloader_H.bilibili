using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Core.Extensions
{
    internal static class GenericExtension
    {
        public static TOut Pipe<TIn, TOut>(this TIn input, Func<TIn, TOut> transform) => transform(input);
    }
}
