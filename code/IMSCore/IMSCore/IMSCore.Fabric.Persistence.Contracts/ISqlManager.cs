using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public interface ISqlManager
    {
        object Execute(IPersistenceItem persistentItem);

        object Execute(IPersistenceItem persistentItem, Action<OperationResponse> action);

        DataSet GetResult(IPersistenceItem persistentItem);
    }
}
