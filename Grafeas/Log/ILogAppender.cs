using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Grafeas
{
    public interface ILogAppender
    {
        void Write(string message);

        string Name { get; set; }

        string Pattern { get; set; }

        void Setup();

        string Path { get; set; }
    }
}
