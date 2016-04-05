using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public class PersistenceItem : IPersistenceItem
    {
        public PersistenceItem()
        {
            this.TransactionIsolationLevel = System.Data.IsolationLevel.Unspecified;
        }
        public string CommandText { get; set; }

        public string ConnectionString { get; set; }

        public string ConnectionStringValue { get; set; }

        public string Identifier { get; set; }

        public ElasticSearchIndexingData IndexData { get; set; }

        public bool IsDependentItem { get; set; }

        public System.Data.IsolationLevel TransactionIsolationLevel { get; set; }

        public bool IsParentItem { get; set; }

        public List<Parameter> Parameters { get; set; }

        public PersistenceType PersistenceType { get; set; }

        public SqlCommandType SqlCommandType { get; set; }

        public TransactionScopeOption TransactionScopeOption { get; set; }
    }
}
