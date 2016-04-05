using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace IMSCore.Infra.Framework.Contracts
{
   public interface IStandardModuleTypesRegistry
    {
        void RegisterType<TFrom, TTo>();
        void RegisterType<TFrom, TTo>(InstanceLifetimeOptions lifetime);
        //void RegisterType<TFrom, TTo>(InstanceLifetimeOptions lifetime, params InjectionMember[] injectionMembers);
    }
}
