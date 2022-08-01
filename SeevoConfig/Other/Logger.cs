using System;
using System.Windows;

namespace SeevoConfig.Other
{
    public static class Logger
    {
        public static void LogAndDisplayError(Exception ex)
        {
            LogAndDisplayError(ex, ex.GetType().Name);
        }

        public static void LogAndDisplayError(Exception ex, string title)
        {
            MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            LogError(ex);
        }

        public static void LogError(Exception ex)
        {
            // do something here
        }
    }
}
