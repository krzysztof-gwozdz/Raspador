using Raspador.Common;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public static class BoardGameGeekScraper
{
    private const string Directory = @"C:\Users\KrzysztofGwozdz\Desktop";

    public static async Task DoIt()
    {
        try
        {
            await AnsiConsole.Progress()
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn()
                )
                .StartAsync(async progressContext =>
                {
                    var downloadTask = progressContext.AddTask("[bold blue]Downloading pages[/]");
                    var scrapTask = progressContext.AddTask("[bold red]Scraping pages[/]");
                    var exportTask = progressContext.AddTask("[bold green]Exporting data[/]");
                    
                    await new BrowsePageDownloader(downloadTask, $@"{Directory}\pages\").Download();
                    var boardGames = await new BrowsePageScraper(scrapTask, $@"{Directory}\pages\").Scrap();
                    await Export(exportTask, boardGames);
                });
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    private static async Task Export(ProgressTask exportTask, List<BoardGame> boardGames)
    {
        exportTask.MaxValue = 2;
        await JsonExporter.Export($@"{Directory}\boardgames_{DateTime.Now:yyyyMMdd_HHmmss}.json", boardGames);
        exportTask.Increment(1);
        await CsvExporter.Export<BoardGame, BoardGameMap>($@"{Directory}\boardgames_{DateTime.Now:yyyyMMdd_HHmmss}.csv", boardGames);
        exportTask.Increment(1);
    }
}