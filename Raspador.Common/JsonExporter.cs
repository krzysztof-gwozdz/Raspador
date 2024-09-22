using System.Text.Json;

namespace Raspador.Common;

public static class JsonExporter
{
    private static JsonSerializerOptions Options => new() { WriteIndented = true };

    public static async Task Export<TData>(string path, IEnumerable<TData> data) =>
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(data, Options));
}