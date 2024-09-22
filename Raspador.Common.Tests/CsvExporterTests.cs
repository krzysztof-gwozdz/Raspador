namespace Raspador.Common.Tests;

public class CsvExporterTests
{
    [Fact]
    public async Task Export_ForValidPathAndData_SavesDataToFile()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();

        // act
        await CsvExporter.Export<Data, DataCsvMap>(path, data);

        // assert
        var actual = await File.ReadAllTextAsync(path);
        actual.Should().Be($"Name,Age{Environment.NewLine}Kris,30{Environment.NewLine}Ana,18{Environment.NewLine}");
    }
}