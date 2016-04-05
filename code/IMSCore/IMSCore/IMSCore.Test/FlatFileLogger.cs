using IMSCore.Common.Logger.Contracts;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Test
{
    [TestClass]
    public class FlatFileLogger
    {
        private IFlatFileLogger LoadDependencies() {
            IEnumerable<IMSCore.Infra.BootStrapper.ResolverOptions> options = new List<IMSCore.Infra.BootStrapper.ResolverOptions>()
            {
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Repository\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Infra\\Debug", SearchPattern="IMSCore.*.dll"},
            };
            IMSCore.Infra.BootStrapper.UnityResolver resolver = new Infra.BootStrapper.UnityResolver(options);
            //return resolver.Resolve<IFlatFileLogger>();
            return resolver.Resolve<IFlatFileLogger>( new ParameterOverride("fileName", "IMSLogger.log"));
        }
        [TestMethod]
        public void TestFlatFileErrorMessage()
        {
            IFlatFileLogger logger = LoadDependencies();            
            logger.Error("This is error message-1");
        }

        [TestMethod]
        public void TestFlatFileWarningMessage()
        {
            IFlatFileLogger logger = LoadDependencies();
            logger.Warning("This is error message-2");
        }
    }
}
