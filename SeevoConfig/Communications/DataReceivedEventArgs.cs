﻿using System;

namespace SeevoConfig.Communications
{
    public class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; private set; }

        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}
