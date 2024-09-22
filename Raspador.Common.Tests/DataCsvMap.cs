using System.Globalization;
using CsvHelper.Configuration;

namespace Raspador.Common.Tests;

public sealed class DataCsvMap: ClassMap<Data>
{
    public DataCsvMap() => AutoMap(CultureInfo.InvariantCulture);
}