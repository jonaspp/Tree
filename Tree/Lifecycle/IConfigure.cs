using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Tree.Configuration;

namespace Tree.Lifecycle
{
    public interface IConfigure
    {
        void Configure(ContainerElement element);
    }
}
