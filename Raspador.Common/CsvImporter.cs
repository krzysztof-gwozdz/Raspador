using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Raspador.Common;

public static class CsvImporter
{
    public static TData[] Import<TData, TDataMap>(string path) where TDataMap : ClassMap
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TDataMap>();
        return csv.GetRecords<TData>().ToArray();
    }
}