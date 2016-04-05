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
    public class ParsistenceSQLManager
    {
        private IPersistenceAccessManager LoadDependencies()
        {
            IEnumerable<IMSCore.Infra.BootStrapper.ResolverOptions> options = new List<IMSCore.Infra.BootStrapper.ResolverOptions>()
            {
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Common\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Infra\\Debug", SearchPattern="IMSCore.*.dll"},
                new IMSCore.Infra.BootStrapper.ResolverOptions { Path="..\\..\\..\\..\\..\\libraries\\Fabric\\Debug", SearchPattern="IMSCore.*.dll"},
            };
            IMSCore.Infra.BootStrapper.UnityResolver resolver = new IMSCore.Infra.BootStrapper.UnityResolver(options);

            return resolver.Resolve<IMSCore.Fabric.Persistence.Contracts.IPersistenceAccessManager>();
        }


        [TestMethod]
        public void GetFromSqlDatabase()
        {
            Fabric.Persistence.Contracts.PersistenceItem sqlGetItem = new Fabric.Persistence.Contracts.PersistenceItem
            {
                CommandText = "Select * from sewadar",
                ConnectionString = "sqlCon",
                SqlCommandType = Fabric.Persistence.Contracts.SqlCommandType.Text,
                PersistenceType = Fabric.Persistence.Contracts.PersistenceType.SQL,
                //Parameters = new Dictionary<string, object>() { { "a", 1 } },                
            };

            var accessMgr = LoadDependencies();

            var result = accessMgr.Get(sqlGetItem);
        }

        [TestMethod]
        public void GetFromSqlDatabaseWithWhereCondition()
        {
            Fabric.Persistence.Contracts.PersistenceItem sqlGetItem = new Fabric.Persistence.Contracts.PersistenceItem
            {
                CommandText = "Select * from sewadar WHERE sewadar_nm like 'dh%'",
                ConnectionString = "sqlCon",
                SqlCommandType = Fabric.Persistence.Contracts.SqlCommandType.Text,
                PersistenceType = Fabric.Persistence.Contracts.PersistenceType.SQL,
                Parameters = new List<Parameter>() {
                    new Parameter() {Name="@name", Value="dh" }
                }
            };

            var accessMgr = LoadDependencies();

            var result = accessMgr.Get(sqlGetItem);
        }

        [TestMethod]
        public void GetFromSqlDatabaseStoredProcedure()
        {
            Fabric.Persistence.Contracts.PersistenceItem sqlGetItem = new Fabric.Persistence.Contracts.PersistenceItem
            {
                CommandText = "QueryZone",
                ConnectionString = "sqlCon",
                SqlCommandType = Fabric.Persistence.Contracts.SqlCommandType.StoredProcedure,
                PersistenceType = Fabric.Persistence.Contracts.PersistenceType.SQL,
                Parameters = new List<Parameter>() {
                    new Parameter() {Name="@zone_Id", Value="1" }
                }
            };

            var accessMgr = LoadDependencies();

            var result = accessMgr.Get(sqlGetItem);
        }

        [TestMethod]
        public void GetFromSqlDatabaseStoredProcedureWithReturnValue()
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

            var result = accessMgr.Get(sqlGetItem);
        }
    }
}
