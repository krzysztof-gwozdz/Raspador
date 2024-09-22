using CsvHelper;

namespace Raspador.Common.Tests;

public class CsvImporterTests
{
    [Fact]
    public void Import_ForValidPathAndData_ReturnsDeserializedData()
    {
        // arrange
        var path = Path.GetTempFileName();
        var data = new TestData();
        File.WriteAllText(path, "Name,Age\nKris,30\nAna,18\n");

        // act
        var result = CsvImporter.Import<Data, DataCsvMap>(path);

        // assert
        result.Should().BeEquivalentTo(data);
    }
    
    [Fact]
    public void Import_ForInvalidPath_ReturnsEmptyArray()
    {
        // arrange
        var path = Path.GetTempFileName();

        // act
        var result = CsvImporter.Import<Data, DataCsvMap>(path);

        // assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void Import_ForInvalidData_ThrowsInvalidOperationException()
    {
        // arrange
        var path = Path.GetTempFileName();
        File.WriteAllText(path, "invalid csv");

        // act
        var act = () => CsvImporter.Import<Data, DataCsvMap>(path);

        // assert
        act.Should().Throw<CsvHelperException>();
    }
}