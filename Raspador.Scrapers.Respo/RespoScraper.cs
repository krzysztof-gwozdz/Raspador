using Raspador.Scrapers.Respo.Models;

namespace Raspador.Scrapers.Respo;

public static class RespoScraper
{
    public static async Task DoIt()
    {   
        // var recipes = HtmlImporter.Import(@"C:\priv\respo");
        // CsvExporter.Export(@"C:\priv\respo.csv", recipes);
        // JsonExporter.Export(@"C:\priv\respo.json", recipes);
        var recipes = await JsonImporter.Import<Recipe>(@"C:\priv\respo.json");

        var allIngredients = recipes.SelectMany(recipe => recipe.Ingredients).OrderBy(ingredient => ingredient.Name).ToArray();
        var groupedIngredients = allIngredients.GroupBy(ingredient => ingredient.Name).Select(ingredients => new
        {
            Name = ingredients.Key,
            All = ingredients,
            Quantity = ingredients.Sum(ingredient => ingredient.Quantity),
            QuantityInfo = string.Join("+", ingredients.Select(ingredient => ingredient.Quantity)),
            Grams = ingredients.Sum(ingredient => ingredient.Grams),
            GramsInfo = string.Join("+", ingredients.Select(ingredient => ingredient.Grams))
        }).OrderByDescending(x => x.Name).ToArray();

        foreach (var ingredient in groupedIngredients)
        {
            Console.WriteLine($"{ingredient.Grams}g {ingredient.Name}");
        }

        Console.WriteLine(groupedIngredients.Length);
    }
}