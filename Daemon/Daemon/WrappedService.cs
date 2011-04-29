using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace Tree.Daemon
{
    public class WrappedService : ServiceBase
    {
        private IWrappedDaemon application = null;

        public WrappedService(IWrappedDaemon app)
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
