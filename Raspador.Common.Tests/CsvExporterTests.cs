namespace Raspador.Common.Tests;

public class CsvExporterTests
{
    [Fact]
    public void Export_ForValidPathAndData_SavesDataToFile()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();

        // act
        CsvExporter.Export<Data, DataCsvMap>(path, data);

        // assert
        var actual = File.ReadAllText(path);
        actual.Should().Be($"Name,Age{Environment.NewLine}Kris,30{Environment.NewLine}Ana,18{Environment.NewLine}");
    }
}