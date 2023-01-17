using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb;

public class EventPayloadDataItem
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string StreamId { get; set; }
        
    public long EventIndex { get; set; }
        
    public string EventTypeName { get; set; }
        
    public JObject EventBody { get; set; }

    public DateTime CreatedOn { set; get; }
}