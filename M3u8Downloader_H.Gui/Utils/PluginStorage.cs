using M3u8Downloader_H.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace M3u8Downloader_H.Gui.Utils
{
    public class PluginStorage(string _rootPath) : IPluginStorage
    {
        public DirectoryInfo CreateDirectory(string Dir)
            => Directory.CreateDirectory(Dir);

        public bool Exists(string path)
            => File.Exists(Path.Combine(_rootPath, path));

        public string GetPath(string path)
            => Path.Combine(_rootPath, path);

        public Stream OpenRead(string path)
            => File.OpenRead(Path.Combine(_rootPath, path));

        public Stream OpenWrite(string path)
            => File.OpenWrite(Path.Combine(_rootPath, path));
    }
}
