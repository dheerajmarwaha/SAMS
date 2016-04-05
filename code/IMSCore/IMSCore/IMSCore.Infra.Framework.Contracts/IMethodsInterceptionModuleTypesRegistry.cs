using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.Framework.Contracts
{
    public interface IMethodsInterceptionModuleTypesRegistry
    {
        void RegisterType<TFrom, TTo>();
        void RegisterType<TFrom, TTo>(InstanceLifetimeOptions lifetime);
        void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects);
        void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects, InstanceLifetimeOptions lifetime);
    }
}
