using IMSCore.Common.Logger.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Test
{
    [TestClass]
    public class Log
    {
        private ILog LoadDependencies() {
            IEnumerable<IMSCore.Infra.BootStrapper.ResolverOptions> options = new List<IMSCore.Infra.BootStrapper.ResolverOptions>()
            {
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Repository\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Infra\\Debug", SearchPattern="IMSCore.*.dll"},
            };
            IMSCore.Infra.BootStrapper.UnityResolver resolver = new Infra.BootStrapper.UnityResolver(options);

            return resolver.Resolve<ILog>();
        }
        [TestMethod]
        public void TestErrorMessage()
        {
            ILog logger = LoadDependencies();
            logger.Error("This is error message");
        }
    }
}
