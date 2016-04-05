using IMSCore.Fabric.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence
{
    public class PersistenceAccessManager : IPersistenceAccessManager
    {
        ISqlManager _injectedSqlManager;

        public PersistenceAccessManager()
        {

        }
        public PersistenceAccessManager(ISqlManager sqlManager)
        {
            this._injectedSqlManager = sqlManager;
        }
        public object Get(IPersistenceItem item)
        {
            object result = new object();
            switch (item.PersistenceType)
            {
                case PersistenceType.SQLAzure:
                    break;
                case PersistenceType.BLOB:
                    break;
                case PersistenceType.MongoDB:
                    break;
                case PersistenceType.DocumentDB:
                    break;
                case PersistenceType.SQL:
                    result = _injectedSqlManager.GetResult(item);
                    break;
                case PersistenceType.EntityFramework:
                    break;
                case PersistenceType.WCF:
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
