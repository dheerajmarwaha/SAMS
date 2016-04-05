using IMSCore.Common.Logger.Contracts;
using IMSCore.Common.SemanticLogger;
using IMSCore.Infra.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.Logger.Provider
{
    [Export(typeof(IModule))]
    public class LoggingModules : IModule
    {
        public void RegisterInterfaceInterceptableTypes(IInterfaceInterceptionModuleTypesRegistry interfaceInterceptionRegistry)
        {
        }

        public void RegisterMethodInterceptableTypes(IMethodsInterceptionModuleTypesRegistry interfaceInterceptionRegistry)
        {
        }

        public void RegisterStandardTypes(IStandardModuleTypesRegistry standardRegistry)
        {
            standardRegistry.RegisterType<ILog, Log>(InstanceLifetimeOptions.PerContainerLife);
            standardRegistry.RegisterType<IFlatFileLogger, FlatFileLogger>(InstanceLifetimeOptions.PerContainerLife);            
            standardRegistry.RegisterType<IDatabaseLogger, SqlDatabaseLogger>(InstanceLifetimeOptions.PerContainerLife);
        }
    }
}
