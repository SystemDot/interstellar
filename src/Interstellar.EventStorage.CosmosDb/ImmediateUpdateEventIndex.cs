namespace Interstellar.EventStorage.CosmosDb;

public class ImmediateUpdateEventIndex
{
    public string Id { get; init; }
    public string Type { get; init; }
    public string StreamId { get; init; }
    public long Index { get; init; }
}