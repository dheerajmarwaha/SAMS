using IMSCore.Fabric.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCore.Fabric.Persistence
{
    public class PersistenceBase
    {
        public Action<OperationResponse> Action { get; set; }
        public IPersistenceItem PersistenceItem { get; set; }
        protected string SqlConnectionString { get; set; }
        public string SqlConnectionStringValue { get; set; }
        protected SqlCommand SqlCommand;
        protected  IsolationLevel TransactionIsolationLevel { get; set; }

        public PersistenceBase()
        {

        }

        protected SqlCommand GetSqlCommand(string command,
                                             string connection,
                                             string connectionValue,
                                             SqlCommandType type,
                                             List<Parameter> parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(command);

            if (!string.IsNullOrEmpty(connectionValue))
            {
                sqlCommand.Connection = new SqlConnection(connectionValue);
            }
            else {
                connectionValue = ConfigurationManager.AppSettings[connection];
                if (string.IsNullOrEmpty(connectionValue)) {
                    throw new Exception("Configuration Entry is missing for key : " + connection);
                }
                sqlCommand.Connection = new SqlConnection(connectionValue);
            }

            switch (type)
            {
                case SqlCommandType.StoredProcedure:
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    break;
                case SqlCommandType.Text:
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    break;
                default:
                    throw new Exception("SqlCommandType must be specified properly");
            }
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    SqlParameter sqlParameter;
                    sqlParameter = sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);

                    sqlParameter.Direction = parameter.Direction;
                    sqlParameter.DbType = parameter.DbType;

                    if (parameter.Size > 0)
                        sqlParameter.Size = parameter.Size;
                }
            }
            return sqlCommand;
        }

        protected void Commit()
        {
            if (this.SqlCommand != null)
            {
                if (this.SqlCommand.Transaction != null) {
                    this.SqlCommand.Transaction.Commit();
                }
                this.SqlCommand.Dispose();
            }
        }

        protected void RollBack() {
            if (this.SqlCommand != null) {
                if (this.SqlCommand.Transaction != null) {
                    this.SqlCommand.Transaction.Rollback();
                }
                this.SqlCommand.Dispose();
            }
        }
    }
}
