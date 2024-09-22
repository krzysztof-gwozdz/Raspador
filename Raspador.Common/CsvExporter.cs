using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Raspador.Common;

public static class CsvExporter
{
    public static void Export<TData, TDataMap>(string path, IEnumerable<TData> data) where TDataMap : ClassMap
    {
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TDataMap>();
        csv.WriteRecords(data);
    }
}