using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMSCore.Infra.Framework.Contracts
{
    public interface IModule
    {
        void RegisterStandardTypes(IStandardModuleTypesRegistry standardRegistry);

        void RegisterInterfaceInterceptableTypes(IInterfaceInterceptionModuleTypesRegistry interfaceInterceptionRegistry);

        void RegisterMethodInterceptableTypes(IMethodsInterceptionModuleTypesRegistry interfaceInterceptionRegistry);
    }
}
