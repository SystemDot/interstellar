using Newtonsoft.Json;

namespace Interstellar.Examples.CosmosDb;

public class WotsitActivity
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; init; }
    public Guid ThingId { get; init; }
    public string State { get; init; } = null!;
}