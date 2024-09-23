using System.Text.RegularExpressions;
using System.Xml.XPath;
using HtmlAgilityPack;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public class BrowsePageScraper(ProgressTask task, string pagesDirectoryPath)
{
    public async Task<List<BoardGame>> Scrap()
    {
        var pagesCount = Directory.GetFiles(pagesDirectoryPath).Length;
        task.MaxValue = pagesCount;
        var boardGames = new List<BoardGame>();
        for (var i = 1; i <= pagesCount; i++)
        {
            var page = await GetPageAsync(i);
            var rows = page.DocumentNode.SelectNodes("//tr")?.Where(x => x.Id.StartsWith("row_"));
            if (rows is null)
            {
                AnsiConsole.MarkupLine("[red]No rows found[/]");
                break;
            }
            foreach (var row in rows)
            {
                var href = row.SelectSingleNode(".//a[@class='primary']")?.Attributes["href"]?.Value;
                var id = href?.Split('/')[2] ?? "0";
                var rank = int.TryParse(row.SelectSingleNode(".//td[@class='collection_rank']")?.InnerText?.Trim() ?? "0", out var rankValue) ? rankValue : null as int?;
                var url = $"https://boardgamegeek.com/{href ?? "TODO"}";
                var title = row.SelectSingleNode(".//a[@class='primary']")?.InnerText?.Trim() ?? "TODO";
                var year = int.TryParse(row.SelectSingleNode(".//span[@class='smallerfont dull']")?.InnerText?.Replace("(", "").Replace(")", "").Trim() ?? "0", out var yearValue) ? yearValue : null as int?;
                var description = row.SelectSingleNode(".//p[@class='smallefont dull']")?.InnerText?.Trim() ?? string.Empty;
                boardGames.Add(new BoardGame(int.Parse(id), rank, title, year, description, url));
            }
            task.Increment(1);
        }

        return boardGames;
    }

    private async Task<HtmlDocument> GetPageAsync(int id)
    {
        var content = await File.ReadAllTextAsync($@"{pagesDirectoryPath}{id}.html");
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);
        return htmlDocument;
    }
}