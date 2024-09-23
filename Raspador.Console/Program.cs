using Raspador.Console;
using Raspador.Scraper.BoardGameGeek;
using Raspador.Scrapers.Respo;
using Spectre.Console;

var scraper = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(Scrapers.All));
AnsiConsole.MarkupLine($"[bold]{scraper}[/]");
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
                switch (scraper)
                {
                    case Scrapers.BoardGameGeek:
                    {
                        await BoardGameGeekScraper.DoIt(progressContext);
                        break;
                    }
                    case Scrapers.Respo:
                    {
                        await RespoScraper.DoIt(progressContext);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown scraper: {scraper}");
                }
            }
        );
}
catch (Exception e)
{
    AnsiConsole.WriteException(e);
    throw;
}