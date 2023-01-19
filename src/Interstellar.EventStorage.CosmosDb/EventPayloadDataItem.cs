using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb;

public class EventPayloadDataItem
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = null!;

    public string StreamId { get; init; } = null!;
    public long EventIndex { get; init; }
    public string EventTypeName { get; init; } = null!;
    public IDictionary<string, object> Headers { get; init; } = null!;
    public JObject EventBody { get; init; } = null!;
    public DateTime CreatedOn { get; init; }
}