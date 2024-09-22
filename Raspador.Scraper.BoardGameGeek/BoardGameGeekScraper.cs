using System.Net;
using Raspador.Common;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public static class BoardGameGeekScraper
{
    private const string Directory = @"C:\Users\KrzysztofGwozdz\Desktop\";

    public static async Task DoIt()
    {
        using var handler = new HttpClientHandler();
        handler.CookieContainer = GetCookieContainer();
        using var httpclient = new HttpClient(handler);

        AnsiConsole.MarkupLine("[bold]Time for scraping![/]");
        var boardGames = await new BrowsePageScraper(httpclient).Scrap();
        AnsiConsole.MarkupLine($"[bold]Scraped {boardGames.Count} board games![/]");

        AnsiConsole.MarkupLine("[bold]Exporting to JSON[/]");
        await JsonExporter.Export($"{Directory}boardgames.json", boardGames);

        await CsvExporter.Export<BoardGame, BoardGameMap>($"{Directory}boardgames.csv", boardGames);
    }

    private static CookieContainer GetCookieContainer()
    {
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("SessionID", "", "/", "boardgamegeek.com"));
        cookieContainer.Add(new Cookie("bggpassword", "", "/", "boardgamegeek.com"));
        cookieContainer.Add(new Cookie("bggusername", "", "/", "boardgamegeek.com"));
        cookieContainer.Add(new Cookie("cc_cookie", "", "/", "boardgamegeek.com"));
        return cookieContainer;
    }
}