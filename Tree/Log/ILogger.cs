using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Log
{
    public interface ILogger
    {
        void Log(string message);

        void Error(string message);

        void Debug(string message);

        void Warn(string message);
    }
}
