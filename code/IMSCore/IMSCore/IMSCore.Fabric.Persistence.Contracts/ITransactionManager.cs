using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public interface ITransactionManager
    {

        TransactionResponse Execute(IEnumerable<IPersistenceItem> persistentItem);
    }
}
