using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.Gui.Models
{
    public partial class ProxyService (string address, string username, string password)
    {
        public string Address { get; set; } = address;

        public string UserName { get; set; } = username;

        public string PassWord { get; set; } = password;

        public ProxyService() : this(string.Empty, string.Empty, string.Empty)
        {
        }

    }
}
