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
    [EventSource(Name = "BasicSQLLogger")]
    public class SqlDatabaseLogger : Log, IDatabaseLogger
    {
        DatabaseLogOptions _LogOptions;
        public DatabaseLogOptions LogOptions
        {
            get
            {
                return _LogOptions;
            }
        }

        public SqlDatabaseLogger()
        {
            _LogOptions = new DatabaseLogOptions
            {
                BufferingCount = 1,
                BufferingInterval = new TimeSpan(0, 0, 0, 0),
                ConnectionString = @"Data Source=DHEERAJ\SQLEXPRESS12;Initial Catalog=tmp;Integrated Security=false;User ID=sa;Password=dera@12345",
                //ConnectionString = @"Data Source=DHEERAJ\SQLEXPRESS12;Initial Catalog=tmp;Integrated Security=True",
                LogTableName = "Traces",
                Name = "DBLog"
            };

            var sqlLog = SqlDatabaseLog.CreateListener(
                _LogOptions.Name,
                _LogOptions.ConnectionString,
                _LogOptions.LogTableName,
                _LogOptions.BufferingInterval,
                _LogOptions.BufferingCount
                );
            
            sqlLog.EnableEvents((EventSource)this, EventLevel.LogAlways);       
            
                
            
        }       
    }
}
