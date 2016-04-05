using IMSCore.Common;
using IMSCore.Fabric.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IMSCore.Fabric.Persistence
{
    public class TransactionManager : ITransactionManager
    {
        private List<OperationResponse> sentResponses;
        private bool isDependentItemsExists;
        private IEnumerable<Fabric.Persistence.Contracts.IPersistenceItem> persistenceItems;


        public TransactionManager()
        {

        }

        
        public IReadOnlyCollection<OperationResponse> SentResponses
        {
            get { return new ReadOnlyCollection<OperationResponse>(this.sentResponses); }
        }

      

        private void SendInternal(OperationResponse response) {
            if (!response.IsRefParamResponse) {
                this.sentResponses.Add(response);
                SetRefValueInIndexingData(response);
            }

            if (this.isDependentItemsExists) {
                SetRefValueInPersistenceitems(response);
            }
        }

        private void SetRefValueInPersistenceitems(OperationResponse response)
        {
            if (response.ContentType == ContentType.SerializedDataSet) {
                DataSet responseDataSet = new DataSet();
                responseDataSet.ReadXml(Utilities.GenerateStreamFromString(Convert.ToString(response.Content)));
                foreach (DataTable dataTable in responseDataSet.Tables)
                {
                    if (dataTable.Rows.Count > 0) {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var refParamName = column.ColumnName + "_Ref";
                            var refParamValue = Convert.ToString(dataTable.Rows[0][column.ColumnName]);

                            foreach (var persistenceItem in this.persistenceItems.Where(i => i.IsDependentItem == true))
                            {
                                if (persistenceItem.SqlCommandType == SqlCommandType.StoredProcedure)
                                {
                                    List<string> keys = new List<string>();
                                    foreach (var refValues in persistenceItem.Parameters.Where(p => Convert.ToString(p.Value) == refParamName))
                                    {
                                        keys.Add(refValues.Name);
                                    }

                                    foreach (var key in keys)
                                    {
                                        var parameter = persistenceItem.Parameters.FirstOrDefault(p => p.Name == key);
                                        if (parameter != null)
                                        {
                                            parameter.Value = refParamValue;
                                        }
                                    }
                                }
                                else if (persistenceItem.SqlCommandType == SqlCommandType.Text) {
                                    persistenceItem.CommandText = persistenceItem.CommandText.Replace(refParamName, refParamValue);
                                }
                            }
                        }
                    }

                }
            }
        }

        private void SetRefValueInIndexingData(OperationResponse response)
        {
            if (response.ContentType == ContentType.SerializedDataSet)
            {
                DataSet responsDataSet = new DataSet();
                responsDataSet.ReadXml(Utilities.GenerateStreamFromString(Convert.ToString(response.Content)));
                foreach (DataTable dataTable in responsDataSet.Tables)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        if (string.Equals(dataTable.TableName, "OutputParams", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (DataRow outparam in dataTable.Rows)
                            {
                                var refParamName = Convert.ToString(outparam["Parameter"]) + "_Ref";
                                refParamName = refParamName.Replace("@", string.Empty);
                                var refParamValue = Convert.ToString(outparam["Value"]);
                                ReplaceParamValues(refParamName, refParamValue);
                            }
                        }
                        else
                        {
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                var refParamName = column.ColumnName + "_Ref";
                                var refParamValue = Convert.ToString(dataTable.Rows[0][column.ColumnName]);
                                ReplaceParamValues(refParamName, refParamValue);
                            }
                        }
                    }
                }
            }
        }

        private void ReplaceParamValues(string refParamName, string refParamValue)
        {
            foreach (var persistenceItem in this.persistenceItems)
            {
                if (persistenceItem.IndexData != null)
                {
                    if (persistenceItem.IndexData.Data != null)
                    {
                        List<string> keys = new List<string>();
                        foreach (var refValues in persistenceItem.IndexData.Data.Where(p => Convert.ToString(p.Value) == refParamName))
                            keys.Add(refValues.Key);

                        foreach (var key in keys)
                        {
                            persistenceItem.IndexData.Data[key] = refParamValue;
                        }
                    }
                }
            }
        }

        public TransactionResponse Execute(IEnumerable<IPersistenceItem> persistentItem)
        {
            this.sentResponses = new List<OperationResponse>();
            this.persistenceItems = persistentItem;
            this.isDependentItemsExists = this.persistenceItems.Any(i => i.IsDependentItem);

            try
            {
                ValidateIsParentFlag();
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        if (this.isDependentItemsExists)
                        {
                            this.persistenceItems = persistenceItems.OrderBy(i => i.Parameters);
                        }

                        foreach (PersistenceItem item in this.persistenceItems)
                        {
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
                                    var sqlManager = Utilities.GetInstance<ISqlManager>();
                                    sqlManager.Execute(item, SendInternal);
                                    break;
                                case PersistenceType.EntityFramework:
                                    break;
                                case PersistenceType.WCF:
                                    break;
                                default:
                                    break;
                            }
                        }
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw ex;
                    }
                }
            }
            catch (Exception ex) {
                List<OperationResponse> response = new List<OperationResponse>();

                var exception = ex;

                List<string> errors = new List<string>();

                while (exception != null)
                {
                    errors.Add(exception.Message);
                    exception = exception.InnerException;
                }

                response.Add(new OperationResponse(ex.Message, errors, Contracts.OperationStatus.Failed, ContentType.String, false));
                return new TransactionResponse() { OperationResponses = response };
            }
            //Wait untill get all the response
            Common.Utilities.RetryUntilSuccessOrTimeout(
                    (() => this.SentResponses.Count >= persistenceItems.Count()),
                     new TimeSpan(0, 0, 10));

            // Initiale Pipeline   
            if (!this.SentResponses.Any(r => r.Status == OperationStatus.Failed))
                new Task(() => InitiatePipeline(this.persistenceItems)).Start();

            return new TransactionResponse() { OperationResponses = this.sentResponses };
        }

        private void ValidateIsParentFlag()
        {
            if (this.isDependentItemsExists) {
                if (!this.persistenceItems.Any(i => i.IsParentItem)) {
                    throw new Exception("If there is a item with IsDependentItem flag set to true, then there should be a item with IsParentItem flag set true");
                }
                if (this.persistenceItems.Count(i => i.IsParentItem) > 1) {
                    throw new Exception("There can be only one item with IsParentItem flag set to true");
                }
            }
        }

        private void InitiatePipeline(IEnumerable<Fabric.Persistence.Contracts.IPersistenceItem> items)
        {
            //var pipeline = Utilities.GetInstance<Fabric.Pipeline.Contracts.IPipeline>();
            //if (pipeline != null)
            //{
            //    var pipleLineConfig = Pipeline.Contracts.Config.RegisteredPipelines.GetConfig();
            //    if (pipleLineConfig != null)
            //    {
            //        foreach (Lego.Fabric.Pipeline.Contracts.Config.Pipeline configuredPipeline in pipleLineConfig.Pipelines)
            //        {
            //            pipeline.Start(configuredPipeline.Name, items);
            //        }
            //    }
            //}
        }
    }
}
