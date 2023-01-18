using System.Reflection;

namespace Interstellar.EventStorage.CosmosDb;

public static class EmbeddedResourceReader
{
    public static string Read(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}