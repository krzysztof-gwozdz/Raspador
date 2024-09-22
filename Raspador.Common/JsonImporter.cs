using System.Text.Json;

namespace Raspador.Common;

public static class JsonImporter
{
    public static TData[] Import<TData>(string path) => 
        JsonSerializer.Deserialize<TData[]>(File.ReadAllText(path)) ?? throw new InvalidOperationException("Failed to deserialize JSON.");
}