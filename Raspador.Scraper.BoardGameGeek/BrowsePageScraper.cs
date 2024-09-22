using HtmlAgilityPack;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public class BrowsePageScraper(HttpClient httpclient)
{
    private const int Pages = 1582;

    public async Task<List<BoardGame>> Scrap()
    {
        var boardGames = new List<BoardGame>();
        for (var i = 1; i <= Pages; i++)
        {
            var page = await GetPageAsync(i);
            var rows = page.DocumentNode.SelectNodes("//tr[@id='row_']");
            if (rows is null)
            {
                AnsiConsole.MarkupLine("[red]No rows found[/]");
                break;
            }

            foreach (var row in rows)
            {
                var href = row.SelectSingleNode(".//a[@class='primary']")?.Attributes["href"]?.Value;
                var id = href?.Split('/')[2] ?? "0";
                var rank = row.SelectSingleNode(".//td[@class='collection_rank']")?.InnerText?.Trim() ?? "0";
                var url = $"https://boardgamegeek.com/{href ?? "TODO"}";
                var title = row.SelectSingleNode(".//a[@class='primary']")?.InnerText?.Trim() ?? "TODO";
                var year = row.SelectSingleNode(".//span[@class='smallerfont dull']")?.InnerText?.Replace("(", "").Replace(")", "").Trim() ?? "0";
                var description = row.SelectSingleNode(".//p[@class='smallefont dull']")?.InnerText?.Trim() ?? string.Empty;
                boardGames.Add(new BoardGame(int.Parse(id), int.Parse(rank), title, int.Parse(year), description, url));
            }

            AnsiConsole.MarkupLine($"[green]Page {i} scraped[/]");
        }

        return boardGames;
    }

    private async Task<HtmlDocument> GetPageAsync(int id)
    {
        var url = $"https://boardgamegeek.com/browse/boardgame/page/{id}";
        var content = await (await httpclient.GetAsync(url)).Content.ReadAsStringAsync();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);
        return htmlDocument;
    }
}