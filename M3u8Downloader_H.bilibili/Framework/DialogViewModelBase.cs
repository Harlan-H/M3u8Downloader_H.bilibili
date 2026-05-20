using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Framework
{
    public abstract partial class DialogViewModelBase<T> : PluginViewModelBase
    {
        public T? DialogResult { get; private set; }

        public event EventHandler? Closed;

        public void Close(T? dialogResult = default)
        {
            DialogResult = dialogResult;
            Closed?.Invoke(this, EventArgs.Empty);
        }

    }

    public abstract class DialogViewModelBase : DialogViewModelBase<bool?>
    {

    }
}
