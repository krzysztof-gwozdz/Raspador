using HtmlAgilityPack;

namespace Raspador.Scraper.BoardGameGeek;

public class BrowsePageScraper(HttpClient httpclient)
{
    public async Task<List<BoardGame>> Scrap()
    {
        var boardGames = new List<BoardGame>();
        for (var j = 0; j < 158; j++)
        {
            for (var i = 1; i < 11; i++)
            {
                var page = await GetPageAsync(j * 10 + i);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(page.Content);

                var rows = htmlDocument.DocumentNode.SelectNodes("//tr[@id='row_']");
                if (rows is null)
                {
                    Console.WriteLine("No rows found");
                    continue;
                }

                foreach (var row in rows)
                {
                    var href = row.SelectSingleNode(".//a[@class='primary']")?.Attributes["href"]?.Value;
                    var id = href?.Split('/')[2] ?? "0";
                    var url = $"https://boardgamegeek.com/{href ?? "TODO"}";
                    var title = row.SelectSingleNode(".//a[@class='primary']")?.InnerText?.Trim() ?? "TODO";
                    var year = row.SelectSingleNode(".//span[@class='smallerfont dull']")?.InnerText?.Replace("(", "").Replace(")", "").Trim() ?? "0";
                    var description = row.SelectSingleNode(".//p[@class='smallefont dull']")?.InnerText?.Trim() ?? string.Empty;
                    var rank = row.SelectSingleNode(".//td[@class='collection_rank']")?.InnerText?.Trim() ?? "0";
                    var boardGame = new BoardGame(int.Parse(id), url, int.Parse(year), title, description, int.Parse(rank));
                    boardGames.Add(boardGame);
                }
            }
            Console.WriteLine($"Pages {j * 10 + 1} to {j * 10 + 10} scraped");
        }

        return boardGames;
    }

    private async Task<Page> GetPageAsync(int id)
    {
        var url = $"https://boardgamegeek.com/browse/boardgame/page/{id}";
        var content = await (await httpclient.GetAsync(url)).Content.ReadAsStringAsync();
        return new Page(id, url, content);
    }
}