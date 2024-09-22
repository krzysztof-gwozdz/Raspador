using System.Text.Json;

namespace Raspador.Common.Tests;

public class JsonExporterTests
{
    private static JsonSerializerOptions Options => new() { WriteIndented = true };

    [Fact]
    public void Export_ForValidPathAndData_SavesDataToFile()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();

        // act
        JsonExporter.Export(path, data);

        // assert
        var actual = File.ReadAllText(path);
        actual.Should().Be(JsonSerializer.Serialize(data, Options));
    }
}