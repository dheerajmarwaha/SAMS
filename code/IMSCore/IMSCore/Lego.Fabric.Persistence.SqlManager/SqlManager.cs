using IMSCore.Fabric.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;

namespace IMSCore.Fabric.Persistence
{
    public class SqlManager : PersistenceBase, ISqlManager, IEnlistmentNotification
    {
        private DataSet ResultDataSet = new DataSet();

        public SqlManager()
        {

        }



        public object Execute(IPersistenceItem persistentItem)
        {
            try
            {
                var sqlCommand = this.GetSqlCommand(persistentItem.CommandText,
                                                    persistentItem.ConnectionString,
                                                    persistentItem.ConnectionStringValue,
                                                    persistentItem.SqlCommandType,
                                                    persistentItem.Parameters);
                using (var sqlConnection = sqlCommand.Connection)
                {
                    ExecuteSqlCommand(ref sqlCommand, ref this.ResultDataSet);
                }

                this.Action(new OperationResponse(this.ResultDataSet.GetXml(),
                                                  null,
                                                  OperationStatus.Successful,
                                                  ContentType.SerializedDataSet,
                                                  true));
            }
            catch (Exception ex)
            {
                this.Action(new OperationResponse(ex.Message,
                                                  null,
                                                  OperationStatus.Failed,
                                                  ContentType.String,
                                                  false));
                throw ex;
            }
            return null;
        }

        public object Execute(IPersistenceItem persistentItem, Action<OperationResponse> action)
        {
            try
            {
                this.Action = action;
                this.PersistenceItem = persistentItem;
                this.TransactionIsolationLevel = persistentItem.TransactionIsolationLevel;
                if (persistentItem.TransactionScopeOption == TransactionScopeOption.Suppress)
                {
                    return Execute(persistentItem);
                }

                if (Transaction.Current != null)
                {
                    Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
                }
            }
            catch (Exception ex)
            {
                this.Action(new OperationResponse(ex.Message,
                                                  null,
                                                  OperationStatus.Failed,
                                                  ContentType.String,
                                                  false));
                throw ex;
            }
            return null;
        }

        public DataSet GetResult(IPersistenceItem persistentItem)
        {
            this.PersistenceItem = persistentItem;
            DataSet result = new DataSet();
            var sqlCommand = this.GetSqlCommand(persistentItem.CommandText,
                                                persistentItem.ConnectionString,
                                                persistentItem.ConnectionStringValue,
                                                persistentItem.SqlCommandType,
                                                persistentItem.Parameters);
            ExecuteSqlCommand(ref sqlCommand, ref result);

            if (sqlCommand.Connection != null)
            {
                if (sqlCommand.Connection.State != ConnectionState.Closed)
                {
                    sqlCommand.Connection.Close();
                }
            }
            return result;
        }

        public void Commit(Enlistment enlistment)
        {
            base.Commit();
            this.Action(new Contracts.OperationResponse(this.ResultDataSet.GetXml()
                                                        , null
                                                        , Contracts.OperationStatus.Successful
                                                        , ContentType.SerializedDataSet
                                                        , false));
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                this.SqlCommand = this.GetSqlCommand(this.PersistenceItem.CommandText,
                                                     this.PersistenceItem.ConnectionString,
                                                     this.PersistenceItem.ConnectionStringValue,
                                                     this.PersistenceItem.SqlCommandType,
                                                     this.PersistenceItem.Parameters);

                ExecuteSqlCommand(ref this.SqlCommand, ref this.ResultDataSet);

                this.Action(new Contracts.OperationResponse(ResultDataSet.GetXml(),
                                                            null,
                                                            OperationStatus.Successful,
                                                            ContentType.SerializedDataSet,
                                                            true));
                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                preparingEnlistment.ForceRollback(ex);
            }
        }

        private void ExecuteSqlCommand(ref SqlCommand sqlCommand, ref DataSet dataset)
        {
            sqlCommand.Connection.Open();
            if (this.PersistenceItem != null)
            {
                if (this.PersistenceItem.TransactionScopeOption != TransactionScopeOption.Suppress)
                {
                    if (this.TransactionIsolationLevel == 0)
                    {
                        sqlCommand.Transaction = sqlCommand.Connection.BeginTransaction();
                    }
                    else {
                        sqlCommand.Transaction = sqlCommand.Connection.BeginTransaction(this.TransactionIsolationLevel);
                    }
                }
            }
            if (sqlCommand.Connection.State != ConnectionState.Open)
            {
                sqlCommand.Connection.Open();
            }

            var dr = sqlCommand.ExecuteReader();

            while (dr.HasRows)
            {
                DataTable dt = new DataTable();
                DataTable dtSchema = dr.GetSchemaTable();
                List<DataColumn> lstCols = new List<DataColumn>();

                if (dtSchema != null)
                {
                    foreach (DataRow drow in dtSchema.Rows)
                    {
                        string colName = Convert.ToString(drow["ColumnName"]);
                        DataColumn column = new DataColumn(colName, (Type)(drow["DataType"]));
                        column.Unique = (bool)drow["IsUnique"];
                        column.AllowDBNull = (bool)drow["AllowDBNull"];
                        column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                        lstCols.Add(column);
                        dt.Columns.Add(column);
                    }
                }

                while (dr.Read())
                {
                    DataRow dataRow = dt.NewRow();
                    for (int i = 0; i < lstCols.Count; i++)
                    {
                        dataRow[(DataColumn)lstCols[i]] = dr[i];
                    }
                    dt.Rows.Add(dataRow);
                }
                dataset.Tables.Add(dt);
                dr.NextResult();
            }
            dr.Close();
            //Get Output Parameters

            DataTable outputParamTable = new DataTable("OutputParams");
            outputParamTable.Columns.Add("Parameter", typeof(string));
            outputParamTable.Columns.Add("Value", typeof(object));

            foreach (SqlParameter parameter in sqlCommand.Parameters)
            {
                if (parameter.Direction != ParameterDirection.Input)
                {
                    outputParamTable.Rows.Add(parameter.ParameterName, parameter.Value);
                }
            }

            if (outputParamTable.Rows.Count > 0)
            {
                dataset.Tables.Add(outputParamTable);
            }
            
        }


        public void Rollback(Enlistment enlistment)
        {
            base.RollBack();
            enlistment.Done();
        }
    }
}
