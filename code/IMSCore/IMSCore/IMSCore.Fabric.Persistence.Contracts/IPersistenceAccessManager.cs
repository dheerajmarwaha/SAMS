using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public interface IPersistenceAccessManager
    {
        /// <summary>
        /// Excecutes command for get operations
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        object Get(IPersistenceItem item);
    }
}
