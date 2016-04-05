using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMSCore.Infra.Framework.Contracts
{
    public interface IResolver
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}
