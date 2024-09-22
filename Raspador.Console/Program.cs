using Raspador.Console;
using Raspador.Scrapers.Respo;
using Spectre.Console;

var scraper = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(Scrapers.All));
AnsiConsole.MarkupLine($"[bold]{scraper}[/]");
switch (scraper)
{
    case Scrapers.Respo:
    {
        await RespoScraper.DoIt();
        break;
    }
    default:
        throw new ArgumentOutOfRangeException($"Unknown scraper: {scraper}");
}