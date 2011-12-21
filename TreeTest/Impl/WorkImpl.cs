using System;
using System.Collections.Generic;
using System.Text;
using Tree.Lifecycle;

namespace TreeTest.Impl
{
    public class WorkImpl : IWork, IStartable
    {
        private string text = "Idle";
        public string Get()
        {
            return text;
        }

        public void Start()
        {
            text = "Started";
        }

        public void Stop()
        {
            text = "Stopped";
        }
    }
}
