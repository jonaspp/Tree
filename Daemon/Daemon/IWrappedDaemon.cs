using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Daemon
{
    public interface IWrappedDaemon
    {        
        void Start(string[] args);
        void Stop();
        void WaitForExit();        
    }
}
