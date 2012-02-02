using System;
using System.Collections.Generic;
using System.Text;

namespace Tree.Daemon
{
    public interface IWrappedDromeas
    {        
        void Start(string[] args);
        void Stop();
        void WaitForExit();        
    }
}
