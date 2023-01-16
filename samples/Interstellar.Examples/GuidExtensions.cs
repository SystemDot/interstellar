namespace Interstellar.Examples;

public static class GuidExtensions 
{
    public static string ToThingWotsitStreamId(this Guid id) => $"{nameof(ThingWotsits)}-{id}";
    public static string ToThingStreamId(this Guid id) => $"{nameof(Thing)}-{id}";
}