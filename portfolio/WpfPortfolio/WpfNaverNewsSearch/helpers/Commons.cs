﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfNaverNewsSearch
{
    public class Commons
    {
        //메트로UI용 메시지박스 실행 메서드
        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(title, message, style, null);
        }
    }
}
