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
    internal class MethodInterceptionTypeRegistrar : RegistrarBase, IMethodsInterceptionModuleTypesRegistry
    {
        internal MethodInterceptionTypeRegistrar(IUnityContainer container): base(container, new Interceptor<VirtualMethodInterceptor>())
        {

        }
    }
}
