namespace Interstellar.EventStorage.CosmosDb;

public static class StringExtensions
{
    public static string ToImmediateDispatchPositionId(this string from)
    {
        return $"immediate-dispatch-position-{from}";
    }

    public static string ToMetaDataId(this string from)
    {
        return $"metadata-{from}";
    }
}