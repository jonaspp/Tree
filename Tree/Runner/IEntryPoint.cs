using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Runner
{
    public interface IEntryPoint
    {        
        void Start(string[] args);
        void Stop();
        void WaitForExit();        
    }
}
