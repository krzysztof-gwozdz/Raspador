using Raspador.Common;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public static class BoardGameGeekScraper
{
    private const string Directory = @"C:\Users\KrzysztofGwozdz\Desktop";

    public static async Task DoIt()
    {
        AnsiConsole.MarkupLine("[bold]Downloading pages[/]");
        await new BrowsePageDownloader($@"{Directory}\pages\").Download();
        AnsiConsole.MarkupLine("[bold]Pages downloaded![/]");
        
        AnsiConsole.MarkupLine("[bold]Time for scraping![/]");
        var boardGames = await new BrowsePageScraper($@"{Directory}\pages\").Scrap();
        AnsiConsole.MarkupLine($"[bold]Scraped {boardGames.Count} board games![/]");

        AnsiConsole.MarkupLine("[bold]Exporting to JSON[/]");
        await JsonExporter.Export($"{Directory}\boardgames_{DateTime.Now:yyyyMMdd_HHmmss}.json", boardGames);

        await CsvExporter.Export<BoardGame, BoardGameMap>($"{Directory}\boardgames_{DateTime.Now:yyyyMMdd_HHmmss}.csv", boardGames);
    }
}