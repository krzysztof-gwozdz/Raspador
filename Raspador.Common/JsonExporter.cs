using System.Text.Json;

namespace Raspador.Common;

public static class JsonExporter
{
    private static JsonSerializerOptions Options => new() { WriteIndented = true };

    public static void Export<TData>(string path, IEnumerable<TData> data) =>
        File.WriteAllText(path, JsonSerializer.Serialize(data, Options));
}