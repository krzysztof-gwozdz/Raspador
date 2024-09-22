using System.Text.Json;

namespace Raspador.Common.Tests;

public class JsonExporterTests
{
    private static JsonSerializerOptions Options => new() { WriteIndented = true };

    [Fact]
    public async Task Export_ForValidPathAndData_SavesDataToFile()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();

        // act
        await JsonExporter.Export(path, data);

        // assert
        var actual = await File.ReadAllTextAsync(path);
        actual.Should().Be(JsonSerializer.Serialize(data, Options));
    }
}