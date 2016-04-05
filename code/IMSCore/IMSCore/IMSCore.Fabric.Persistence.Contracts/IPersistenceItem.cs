using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public interface IPersistenceItem
    {
        string ConnectionString { get; set; }
        string ConnectionStringValue { get; set; }
        string CommandText { get; set; }
        SqlCommandType SqlCommandType { get; set; }
        List<Parameter> Parameters { get; set; }
        ElasticSearchIndexingData IndexData { get; set; }
        string Identifier { get; set; }
        TransactionScopeOption TransactionScopeOption { get; set; }
        PersistenceType PersistenceType { get; set; }
        bool IsDependentItem { get; set; }
        bool IsParentItem { get; set; }
        System.Data.IsolationLevel TransactionIsolationLevel { get; set; }
    }
}
