using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.Logger.Contracts
{
    public class DatabaseLogOptions
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string LogTableName { get; set; }

        public int BufferingCount { get; set; }

        public TimeSpan BufferingInterval { get; set; }
    }
}
