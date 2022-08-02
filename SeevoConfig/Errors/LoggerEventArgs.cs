using System;

namespace SeevoConfig.Errors
{
    public class LoggerEventArgs : EventArgs

    {
        public string Message { get; private set; }

        public LoggerEventArgs(string message)
        {
            Message = message;
        }
    }
}
