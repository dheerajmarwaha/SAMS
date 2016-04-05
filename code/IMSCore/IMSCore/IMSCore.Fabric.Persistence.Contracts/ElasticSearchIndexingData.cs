using System.Collections.Generic;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public class ElasticSearchIndexingData
    {
        public string Index { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string FacilityId { get; set; }
        public string TenancyId { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}