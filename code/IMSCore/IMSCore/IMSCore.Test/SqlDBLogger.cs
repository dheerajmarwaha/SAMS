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
    public class SqlDBLogger
    {
        private IDatabaseLogger LoadDependencies() {
            IEnumerable<IMSCore.Infra.BootStrapper.ResolverOptions> options = new List<IMSCore.Infra.BootStrapper.ResolverOptions>()
            {
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Repository\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Infra\\Debug", SearchPattern="IMSCore.*.dll"},
            };
            IMSCore.Infra.BootStrapper.UnityResolver resolver = new Infra.BootStrapper.UnityResolver(options);

            return resolver.Resolve<IDatabaseLogger>();
        }
        [TestMethod]
        public void TestSqlDBErrorMessage()
        {
            IDatabaseLogger logger = LoadDependencies();            
            logger.Error("This is error message-1");
            logger.Error("This is error message-1");
            logger.Error("This is error message-1");
            logger.Error("This is error message-1");
            logger.Error("This is error message-2");
            logger.Error("This is error message-3");
            logger.Error("This is error message-4");
            logger.Error("This is error message-5");
            logger.Error("This is error message-6");
            logger.Error("This is error message-7");
            logger.Error("This is error message-8");

            logger.Error("This is error message-9");
            logger.Error("This is error message-10");
            Console.ReadLine();
        }
    }
}
