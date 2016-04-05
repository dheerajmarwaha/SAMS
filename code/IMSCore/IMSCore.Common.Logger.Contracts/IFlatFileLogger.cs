using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.Logger.Contracts
{
    public interface IFlatFileLogger : ILog
    {
        FileLogOptions LogOptions { get; }
    }
}
