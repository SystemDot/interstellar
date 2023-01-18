using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb;

public static class JObjectExtensions
{
    public static object FromJson(this JObject from, Type eventType)
    {
        return from.ToObject(eventType);
    }
}