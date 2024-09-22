using System.Text.Json;

namespace Raspador.Common;

public static class JsonImporter
{
    public static async Task<TData[]> Import<TData>(string path) => 
        JsonSerializer.Deserialize<TData[]>(await File.ReadAllTextAsync(path)) ?? throw new InvalidOperationException("Failed to deserialize JSON.");
}