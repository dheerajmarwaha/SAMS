using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.Framework.Contracts
{
    public interface IInterfaceInterceptionModuleTypesRegistry
    {
        void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects);
        void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects, InstanceLifetimeOptions lifetime);
    }
}
