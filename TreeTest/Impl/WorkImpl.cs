using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace TreeTest.Impl
{
    public class WorkImpl : IWork, IStart
    {
        private string attrib = "Idle";

        public string Get()
        {
            return attrib;
        }

        public void Start()
        {
            attrib = "Started";
        }

        public void Stop()
        {
            attrib = "Stopped";
        }
    }
}
