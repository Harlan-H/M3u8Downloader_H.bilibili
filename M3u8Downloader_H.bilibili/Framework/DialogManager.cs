using DialogHostAvalonia;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Framework
{
    public class DialogManager
    {
        public DialogManager()
        {

        }

        public static async Task<T?> ShowDialogAsync<T>(DialogViewModelBase<T> dialogScreen)
        {
            void OnDialogOpened(object? openSender, DialogOpenedEventArgs openArgs)
            {
                void OnScreenClosed(object? openSender, EventArgs closeArgs)
                {
                    openArgs.Session.Close();
                    dialogScreen.Closed -= OnScreenClosed;
                }
                dialogScreen.Closed += OnScreenClosed;
            }

            await DialogHost.Show(dialogScreen, "MainDialog", OnDialogOpened);

            return dialogScreen.DialogResult;
        }
    }
}
