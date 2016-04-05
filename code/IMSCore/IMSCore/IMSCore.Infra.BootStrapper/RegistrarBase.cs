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
    internal abstract class RegistrarBase
    {
        private readonly IUnityContainer _container;
        private readonly Interceptor _interceptor;

        internal RegistrarBase(IUnityContainer container)
        {
            this._container = container;
        }

        internal RegistrarBase(IUnityContainer container, Interceptor interceptor)
        {
            this._container = container;
            this._interceptor = interceptor;
        }

        public virtual void RegisterType<TFrom, TTo>() {
            if (_container.IsRegistered<TFrom>() == false) {
                _container.RegisterType(typeof(TFrom), typeof(TTo));
            }
        }

        public virtual void RegisterType<TFrom, TTo>(InstanceLifetimeOptions lifetime) {
            if(_container.IsRegistered<TFrom>() == false){
                _container.RegisterType(typeof(TFrom), typeof(TTo), GetLifeTimeManager(lifetime));
            }
        }

        //public virtual void RegisterType<TFrom, TTo>(InstanceLifetimeOptions lifetime, params InjectionMember[] injectionMembers)
        //{
        //    if (_container.IsRegistered<TFrom>() == false)
        //    {
        //        _container.RegisterType(typeof(TFrom), typeof(TTo), GetLifeTimeManager(lifetime), injectionMembers);
        //    }
        //}

        private LifetimeManager GetLifeTimeManager(InstanceLifetimeOptions options)
        {
            LifetimeManager lifetimeManager = null;
            switch (options)
            {
                case InstanceLifetimeOptions.Transient:
                    lifetimeManager = new PerResolveLifetimeManager();
                    break;
                case InstanceLifetimeOptions.PerThreadLife:
                    lifetimeManager = new PerThreadLifetimeManager();
                    break;
                case InstanceLifetimeOptions.PerContainerLife:
                    lifetimeManager = new ContainerControlledLifetimeManager();
                    break;
                default:
                    lifetimeManager = new PerResolveLifetimeManager();
                    break;
            }
            return lifetimeManager;
        }

        public virtual void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects) {
            if (_container.IsRegistered<TFrom>() == false) {
                this._container.AddNewExtension<Interception>();
                var injections = GetInjections(GetLifeTimeManager(InstanceLifetimeOptions.Transient), aspects);
                this._container.RegisterType(typeof(TFrom), typeof(TTo), injections.ToArray());
            }
        }

        private IList<InjectionMember> GetInjections(LifetimeManager lifetimeManager, IEnumerable<Type> aspects)
        {
            IList<InterceptionBehavior> behaviors = new List<InterceptionBehavior>();
            aspects.ToList().ForEach(t => behaviors.Add(new InterceptionBehavior(t)));

            IList<InjectionMember> injections = new List<InjectionMember>();
            injections.Add(_interceptor);
            behaviors.ToList().ForEach(b => injections.Add(b));

            return injections;
        }

        public virtual void RegisterType<TFrom, TTo>(IEnumerable<Type> aspects, InstanceLifetimeOptions lifetime) {
            if (_container.IsRegistered<TFrom>() == false) {
                this._container.AddNewExtension<Interception>();
                var injections = GetInjections(GetLifeTimeManager(lifetime), aspects);
                this._container.RegisterType(typeof(TFrom), typeof(TTo), injections.ToArray());
            }
        }
    }
}
