﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Grafeas
{
    public interface ILogger
    {
        void Log(string message);

        void Error(string message);

        void Debug(string message);

        void Warn(string message);

        void Log(string message, params object[] args);

        void Error(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Warn(string message, params object[] args);
    }
}
