using System;
using System.Collections.Generic;

namespace IMSCore.Fabric.Persistence.Contracts
{
    [Serializable]
    public class TransactionResponse
    {
        public List<OperationResponse> OperationResponses { get; set; }
        public string SingalRConectionID { get; set; }
    }
}