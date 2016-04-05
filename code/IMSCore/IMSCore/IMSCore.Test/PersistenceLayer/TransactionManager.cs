using IMSCore.Fabric.Persistence.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Test
{
    [TestClass]
    public class TransactionManager
    {
        private ITransactionManager LoadDependencies()
        {
            IEnumerable<IMSCore.Infra.BootStrapper.ResolverOptions> options = new List<IMSCore.Infra.BootStrapper.ResolverOptions>()
            {
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Common\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Infra\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Fabric\\Debug", SearchPattern="IMSCore.*.dll"},
            };
            IMSCore.Infra.BootStrapper.UnityResolver resolver = new IMSCore.Infra.BootStrapper.UnityResolver(options);

            return resolver.Resolve<IMSCore.Fabric.Persistence.Contracts.ITransactionManager>();
        }


      
        [TestMethod]
        public void InsertToSqlDatabaseUsingSPWithReturnValue()
        {
            Fabric.Persistence.Contracts.PersistenceItem sqlGetItem = new Fabric.Persistence.Contracts.PersistenceItem
            {
                CommandText = "InsertZone",
                ConnectionString = "sqlCon",
                SqlCommandType = Fabric.Persistence.Contracts.SqlCommandType.StoredProcedure,
                PersistenceType = Fabric.Persistence.Contracts.PersistenceType.SQL,
                Parameters = new List<Parameter>() {
                    new Parameter() {Name="@zone_Id", Value=null, Direction= System.Data.ParameterDirection.Output,Size=4 },
                    new Parameter() {Name="@zone_cd", Value="cd" },
                    new Parameter() {Name="@zone_nm", Value="demo" },
                    new Parameter() {Name="@remarks", Value="remarks" },
                    new Parameter() {Name="@audit_log_id", Value="1" }
                },
                Identifier ="1"
            };

            var accessMgr = LoadDependencies();

            var result = accessMgr.Execute( new List<PersistenceItem> { sqlGetItem });
        }
    }
}
