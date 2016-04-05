using IMSCore.Infra.Framework.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.BootStrapper
{
    public class UnityResolver : IResolver, IDisposable
    {
        readonly IUnityContainer _container;
        readonly bool _doesResolverOwnsContainer = false;

        public IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public UnityResolver(IEnumerable<ResolverOptions> options)
        {
            _container = new UnityContainer();
            _doesResolverOwnsContainer = true;
            this.LoadModules(options);
            SetResolverFactory();
        }

        public UnityResolver(IEnumerable<ResolverOptions> options, IUnityContainer container)
        {
            _container = container;
            _doesResolverOwnsContainer = false;
            LoadModules(options);
            SetResolverFactory();
        }

        [ImportMany(typeof(IModule))]
        private IEnumerable<Lazy<IModule>> Modules = null;

        private void LoadModules(IEnumerable<ResolverOptions> options)
        {
            using (AggregateCatalog aggregateCatalog = new AggregateCatalog()) {
                options.ToList<ResolverOptions>().ForEach(option =>
                {
                    aggregateCatalog.Catalogs.Add(new DirectoryCatalog(option.Path, option.SearchPattern));
                });

                using (CompositionContainer mefContainer = new CompositionContainer(aggregateCatalog)) {
                    mefContainer.ComposeParts(this);
                    var standardRegister = new StandardTypesRegistrar(_container);
                    var interfaceInterceptionRegistrar = new InterfaceInterceptionTypeRegistrar(_container);
                    var methodInterfaceRegistrar = new MethodInterceptionTypeRegistrar(_container);

                    foreach (Lazy<IModule> module in this.Modules) {
                        module.Value.RegisterStandardTypes(standardRegister);
                        module.Value.RegisterInterfaceInterceptableTypes(interfaceInterceptionRegistrar);
                        module.Value.RegisterMethodInterceptableTypes(methodInterfaceRegistrar);
                    }
                }
            }
        }

        private void SetResolverFactory()
        {
            ResolverFactory.SetResolver(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_doesResolverOwnsContainer) {
                    _container.Dispose();
                }
            }
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(params ResolverOverride[] overrides)
        {
            return _container.Resolve<T>(overrides);
        }


        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }


    }
}
