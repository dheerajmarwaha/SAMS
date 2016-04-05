using IMSCore.Infra.Framework.Contracts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.BootStrapper
{
    internal class InterfaceInterceptionTypeRegistrar : RegistrarBase, IInterfaceInterceptionModuleTypesRegistry
    {
        internal InterfaceInterceptionTypeRegistrar(IUnityContainer container): base(container, new Interceptor<InterfaceInterceptor>())
        {

        }
    }
}
