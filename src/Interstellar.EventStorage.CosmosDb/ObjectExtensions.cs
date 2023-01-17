using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb;

public static class ObjectExtensions
{
    public static JObject ToJson(this object from)
    {
        return JObject.FromObject(from);
    }
}