using System.Globalization;
using CsvHelper.Configuration;

namespace Raspador.Scraper.BoardGameGeek;

public record BoardGame(int Id, int Rank, string Title, int Year, string Description, string Url)
{
    public override string ToString() => $"{Rank}: {Title} [{Id:000000}] ({Year})";
}

public sealed class BoardGameMap : ClassMap<BoardGame>
{
    public BoardGameMap() => AutoMap(CultureInfo.InvariantCulture);
}