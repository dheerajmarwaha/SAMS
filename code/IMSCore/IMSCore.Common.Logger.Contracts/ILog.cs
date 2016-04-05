using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.Logger.Contracts
{
    public interface ILog
    {
        void Critical(string message);
        void Error(string message);
        void Warning(string message);
        void Information(string message);
        void Verbose(string message);
    }
}
