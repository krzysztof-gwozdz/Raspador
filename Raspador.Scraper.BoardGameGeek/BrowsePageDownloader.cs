using System.Net;
using Spectre.Console;

namespace Raspador.Scraper.BoardGameGeek;

public class BrowsePageDownloader(ProgressTask task, string pagesDirectoryPath)
{
    private const int Pages = 1582;

    public async Task Download()
    {
        task.MaxValue = Pages;
        using var handler = new HttpClientHandler();
        handler.CookieContainer = GetCookieContainer();
        using var httpclient = new HttpClient(handler);

        var indexes = Enumerable.Range(1, Pages).ToArray();
        foreach (var index in indexes)
        {
            var url = $"https://boardgamegeek.com/browse/boardgame/page/{index}";
            var content = await (await httpclient.GetAsync(url)).Content.ReadAsStringAsync();
            if (content.Contains("Welcome back, sign in!"))
            {
                AnsiConsole.MarkupLine("[red]Not logged in[/]");
                break;
            }
            if (content.Contains("Error: You are browsing too fast. Please slow down"))
            {
                AnsiConsole.MarkupLine("[darkorange]Too fast[/]");
                AnsiConsole.MarkupLine("[darkorange]Retrying...[/]");
                await Task.Delay(new Random().Next(15000, 30000));
                content = await (await httpclient.GetAsync(url)).Content.ReadAsStringAsync();
                if (content.Contains("Error: You are browsing too fast. Please slow down"))
                {
                    AnsiConsole.MarkupLine("[red]Giving up[/]");
                    break;
                }
            }
            await File.WriteAllTextAsync($"{pagesDirectoryPath}{index}.html", content);
            task.Increment(1);
            await Task.Delay(new Random().Next(1500, 2500));
        }
    }

    private static CookieContainer GetCookieContainer()
    {
        const string domain = "boardgamegeek.com";
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Cookie("SessionID", "", "/", domain));
        cookieContainer.Add(new Cookie("bggpassword", "", "/", domain));
        cookieContainer.Add(new Cookie("bggusername", "", "/", domain));
        cookieContainer.Add(new Cookie("cc_cookie", "", "/", domain));
        return cookieContainer;
    }
}