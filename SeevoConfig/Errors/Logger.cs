using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace SeevoConfig.Errors
{
    public static class Logger
    {
        static readonly string separator = "".PadRight(100, '-');
        public delegate void LoggerEventHandler(LoggerEventArgs e);
        public static event LoggerEventHandler LoggerEvent;

        public static void LogDebug(string message, bool displayMessage = false)
        {
            var logText = GetMessage(message, (string)null);
            LogToDebug(logText);
            LogLogToFile(logText);
            LogToEvent(message);
            DisplayDebug(message, displayMessage);
        }

        public static void LogError(string message, bool displayMessage = true)
        {
            LogError(message, (string)null, displayMessage);
        }

        public static void LogError(Exception ex, bool displayMessage = true)
        {
            LogError(ex.Message, ex, displayMessage);
        }

        public static void LogError(string message, string details, bool displayMessage = true)
        {
            var logText = GetMessage(message, details);
            LogToDebug(logText);
            LogLogToFile(logText);
            LogToEvent(message);
            DisplayError(message, displayMessage);
        }

        public static void LogError(string message, Exception ex, bool displayMessage = true)
        {
            var logText = GetMessage(message, ex);
            LogToDebug(logText);
            LogLogToFile(logText);
            LogToEvent(message);
            DisplayError(message, displayMessage);
        }

        private static void DisplayDebug(string message, bool displayMessage)
        {
            if (!displayMessage) { return; }
            MessageBox.Show(message, "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void DisplayError(string message, bool displayMessage)
        {
            if (!displayMessage) { return; }
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void LogToDebug(string msg)
        {
            Debug.WriteLine(msg);
        }
        private static void LogToEvent(string msg)
        {
            LoggerEvent?.Invoke(new LoggerEventArgs(msg));
        }

        private static void LogLogToFile(string msg)
        {
            var filePath = Path.Combine(GetLogsFolderPath(), $"log-{DateTime.Now.ToString("yyyy-MM-dd")}.log");
            File.AppendAllText(filePath, msg);
        }

        private static string GetLogsFolderPath()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var projPath = Path.Combine(basePath, "..", "..", "..");
            var tempPath = File.Exists(Path.Combine(projPath, $"{nameof(SeevoConfig)}.csproj")) ? projPath : basePath;
            var logPath = Path.Combine(tempPath, "Logs");
            Directory.CreateDirectory(logPath);
            return logPath;
        }

        private static string GetMessage(string message, Exception ex)
        {
            return GetMessage(message, ex?.Message, ex?.GetBaseException().Message, ex?.StackTrace);
        }

        private static string GetMessage(string message, string details)
        {
            return GetMessage(message, null, null, details);
        }

        private static string GetMessage(string message, string exception, string baseException, string details)
        {
            var sb = new StringBuilder();
            sb.AppendLine(separator);
            sb.Append(DateTime.Now.ToString("u"));
            sb.Append(' ');
            sb.AppendLine(message);
            if (!string.IsNullOrEmpty(exception) && message != exception)
            {
                sb.AppendLine();
                sb.Append("Exception: ");
                sb.AppendLine(exception);
            }
            if (!string.IsNullOrEmpty(baseException) && message != baseException && exception != baseException)
            {
                sb.AppendLine();
                sb.Append("Base exception: ");
                sb.AppendLine(baseException);
            }
            if (!string.IsNullOrEmpty(details))
            {
                sb.AppendLine(details);
            }
            sb.AppendLine(separator);
            return sb.ToString();
        }
    }
}
