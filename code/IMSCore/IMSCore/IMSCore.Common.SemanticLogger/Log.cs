using IMSCore.Common.Logger.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.SemanticLogger
{
    public class Log : EventSource, ILog
    {
       
        [Event(1, Message ="{0}", Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            if (IsEnabled()) WriteEvent(1, message);
        }

        [Event(2, Message = "{0}", Level = EventLevel.Error)]
        public void Error(string message)
        {
            if (IsEnabled()) WriteEvent(2, message);
        }

        [Event(3, Message = "{0}", Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            if (IsEnabled()) WriteEvent(3, message);
        }

        [Event(4, Message = "{0}", Level = EventLevel.Informational)]
        public void Information(string message)
        {
            if (IsEnabled()) WriteEvent(4, message);
        }

        [Event(5, Message = "{0}", Level = EventLevel.Verbose)]
        public void Verbose(string message)
        {
            if (IsEnabled()) WriteEvent(5, message);
        }        
    }
}
