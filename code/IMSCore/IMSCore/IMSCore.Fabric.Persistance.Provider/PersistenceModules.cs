using IMSCore.Fabric.Persistence;
using IMSCore.Fabric.Persistence.Contracts;
using IMSCore.Infra.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence.Provider
{
    [Export(typeof(IModule))]
    public class PersistenceModules : IModule
    {
        public void RegisterInterfaceInterceptableTypes(IInterfaceInterceptionModuleTypesRegistry interfaceInterceptionRegistry)
        {
        }

        public void RegisterMethodInterceptableTypes(IMethodsInterceptionModuleTypesRegistry methodInterceptionRegistry)
        {
            var aspects = new List<Type>() {
                typeof(Infra.AspectBehaviors.Caching<Infra.Aspects.Contracts.ICache>)
            };
            methodInterceptionRegistry.RegisterType<Fabric.Persistence.Contracts.ITransactionManager, Fabric.Persistence.TransactionManager>(aspects, InstanceLifetimeOptions.Transient);
        }

        public void RegisterStandardTypes(IStandardModuleTypesRegistry standardRegistry)
        {
            standardRegistry.RegisterType<ISqlManager, SqlManager>();
            standardRegistry.RegisterType<ITransactionManager, TransactionManager>();
            standardRegistry.RegisterType<IPersistenceAccessManager, PersistenceAccessManager>();
        }
    }
}
