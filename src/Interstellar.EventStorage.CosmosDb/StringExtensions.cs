using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb;

public static class StringExtensions
{
    public static object FromJson(this JObject from, Type eventType)
    {
        return from.ToObject(eventType);
    }
}