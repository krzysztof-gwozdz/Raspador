using System.Text.Json;

namespace Raspador.Common.Tests;

public class JsonImporterTests
{
    [Fact]
    public async Task Import_ForValidPathAndData_ReturnsDeserializedData()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(data));

        // act
        var result = await JsonImporter.Import<Data>(path);

        // assert
        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void Import_ForInvalidPath_ThrowsInvalidOperationException()
    {
        // arrange
        var path = Path.GetTempFileName();

        // act
        var act = () => JsonImporter.Import<Data>(path);

        // assert
        act.Should().ThrowAsync<JsonException>();
    }

    [Fact]
    public void Import_ForInvalidData_ThrowsJsonException()
    {
        // arrange
        var path = Path.GetTempFileName();
        File.WriteAllText(path, "invalid json");

        // act
        var act = () => JsonImporter.Import<Data>(path);

        // assert
        act.Should().ThrowAsync<JsonException>();
    }
}