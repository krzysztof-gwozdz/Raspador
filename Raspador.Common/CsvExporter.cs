using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Raspador.Common;

public static class CsvExporter
{
    public static async Task Export<TData, TDataMap>(string path, IEnumerable<TData> data) where TDataMap : ClassMap
    {
        await using var writer = new StreamWriter(path);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TDataMap>();
        await csv.WriteRecordsAsync(data);
    }
}