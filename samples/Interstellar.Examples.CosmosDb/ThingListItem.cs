using Newtonsoft.Json;

namespace Interstellar.Examples.CosmosDb;

public class ThingListItem
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public int Type { get; init; }
}