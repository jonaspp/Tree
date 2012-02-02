using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Tree.Runner;

namespace Tree.Runner
{
    public class WrappedService : ServiceBase
    {
        private IEntryPoint application = null;

        public WrappedService(IEntryPoint app)
        {           

            this.application = app;
        }

        protected override void OnStart(string[] args)
        {
            application.Start(args);
        }

        protected override void OnStop()
        {
            application.Stop();
        }
    }
}
