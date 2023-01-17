using Newtonsoft.Json;

namespace Interstellar.EventStorage.CosmosDb;

public class ImmediateDispatchPosition
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; init; } = null!;
    public string StreamId { get; init; } = null!;
    public long LastIndexDispatched { get; init; }
}