using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Tree.Lifecycle
{
    interface IConfigure
    {
        void Configure(ConfigurationElement element);
    }
}
