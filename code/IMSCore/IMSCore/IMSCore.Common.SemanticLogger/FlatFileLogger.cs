using IMSCore.Common.Logger.Contracts;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Common.SemanticLogger
{
    [EventSource(Name = "FlatFileLog")]
    public sealed class FlatFileLogger : EventSource, IFlatFileLogger
    {
        readonly FileLogOptions _LogOption;

        public FlatFileLogger()
        {
            _LogOption = new FileLogOptions { FileName = "FlatFileLog.log", IsAsync = true };
            var textLog = FlatFileLog.CreateListener(_LogOption.FileName, null, _LogOption.IsAsync);
            textLog.EnableEvents((EventSource)this, EventLevel.LogAlways);
        }

        public FlatFileLogger(string fileName)
        {
            _LogOption = new FileLogOptions { FileName = fileName, IsAsync = true };
            var textLog = FlatFileLog.CreateListener(_LogOption.FileName, null, _LogOption.IsAsync);
            textLog.EnableEvents((EventSource)this, EventLevel.LogAlways);
        }

        public FileLogOptions LogOptions
        {
            get
            {
                return _LogOption;
            }
        }

        [Event(1, Message = "{0}", Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            if (IsEnabled()) WriteEvent(1, message);
        }

        [Event(2, Message = "{0}", Level = EventLevel.Error)]
        public void Error(string message)
        {
            if (IsEnabled()) WriteEvent(2, message);
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

        [Event(3, Message = "{0}", Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            if (IsEnabled()) WriteEvent(3, message);
        }
    }
}
