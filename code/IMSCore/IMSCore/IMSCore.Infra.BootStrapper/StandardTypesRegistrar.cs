using IMSCore.Infra.Framework.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Infra.BootStrapper
{
    internal class StandardTypesRegistrar : RegistrarBase, IStandardModuleTypesRegistry
    {
        internal StandardTypesRegistrar(IUnityContainer container): base(container)
        {

        }
    }
}
