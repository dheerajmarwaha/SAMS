using System;
using System.Collections.Generic;

namespace IMSCore.Fabric.Persistence.Contracts
{
    [Serializable]
    public class OperationResponse
    {
        public object Content { get; set; }
        public List<string> Errors { get; set; }
        public OperationStatus Status { get; set; }
        public string Identifier { get; set; }
        public ContentType ContentType { get; set; }
        public bool IsRefParamResponse { get; set; }

        public OperationResponse(object content, 
                                List<string> errors,
                                OperationStatus status, 
                                ContentType contentType, 
                                bool IsRefParamResponse)
        {
            this.Content = content;
            this.Errors = errors;
            this.Status = status;
            this.ContentType = contentType;
            this.IsRefParamResponse = IsRefParamResponse;
        }

    }
}