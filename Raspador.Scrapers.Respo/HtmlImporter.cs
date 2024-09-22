using HtmlAgilityPack;
using Raspador.Scrapers.Respo.Models;

namespace Raspador.Scrapers.Respo;

public static class HtmlImporter
{
    public static Recipe[] Import(string path)
    {
        var files = Directory.GetFiles(path, "*.html").ToArray();
        var recipes = new List<Recipe>();
        for (var index = 0; index < files.Length; index++)
        {
            try
            {
                var file = files[index];
                var document = GetDocument(file);
                recipes.Add(new Recipe(index + 1,
                    GetName(file),
                    GetPortionAmount(document),
                    GetPrepareTime(document),
                    GetTotalPrepareTime(document),
                    GetDifficultyInfo(document),
                    GetNutritions(document),
                    GetIngredients(document)));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing file: {files[index]}, error: {e.Message}");
            }
        }

        return recipes.ToArray();
    }

    private static string GetName(string filePath) => Path.GetFileNameWithoutExtension(filePath);

    private static Nutritions GetNutritions(HtmlDocument htmlDocument)
    {
        var nutritions = htmlDocument.DocumentNode.SelectNodes("//div[@class='recipe-calculator-section__singleMicro']//span[@class='recipe-calculator-section__singleMicroValue']//span");
        var calories = nutritions.Count > 1 ? int.TryParse(nutritions[0].InnerHtml, out var result) ? result : 0 : 0;
        var protein = nutritions.Count > 1 ? int.TryParse(nutritions[1].InnerHtml, out result) ? result : 0 : 0;
        var fat = nutritions.Count > 2 ? int.TryParse(nutritions[2].InnerHtml, out result) ? result : 0 : 0;
        var carbohydrates = nutritions.Count > 3 ? int.TryParse(nutritions[3].InnerHtml, out result) ? result : 0 : 0;
        var sodium = nutritions.Count > 4 ? int.TryParse(nutritions[4].InnerHtml, out result) ? result : 0 : 0;
        var fiber = nutritions.Count > 5 ? int.TryParse(nutritions[5].InnerHtml, out result) ? result : 0 : 0;
        return new Nutritions(calories, protein, fat, carbohydrates, sodium, fiber);
    }

    private static Ingredient[] GetIngredients(HtmlDocument htmlDocument)
    {
        var ingredientTableRows = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='ingredients-table']//tr").SelectNodes("//td").Where(td => !td.InnerText.Contains(":")).ToArray();
        var ingredients = new List<Ingredient>();
        for (var i = 0; i + 1 < ingredientTableRows.Length; i += 2)
        {
            if (ingredientTableRows[i].InnerText.Replace("&nbsp;", " ").Trim().Replace("\n \n \n", "").Trim() is "Dodatki" or "" )
            {
                i--;
                continue;
            }
            var quantity = double.TryParse(ingredientTableRows[i].InnerText.Replace("&nbsp;", " ").Trim().Replace("\n \n \n", "").Trim().Replace("g", "").Replace(",", "."), out var quantity2) ? quantity2 : -1;
            var nameWithGrams = ingredientTableRows[i + 1].InnerText.Replace("&nbsp;", " ").Trim().Split("\n \n \n");
            var name = nameWithGrams.Length > 0 ? nameWithGrams[0].Trim() : "???";
            var grams = nameWithGrams.Length > 1
                ? double.TryParse(nameWithGrams[1].Replace("g", "").Trim().Replace(",", "."), out var grams2)
                    ? grams2
                    : -1
                : double.TryParse(nameWithGrams[0].Replace("g", "").Trim().Replace(",", "."), out var grams3)
                    ? grams3
                    : (double?)null;
            if (quantity == -1 || grams == -1)
            {
                Console.WriteLine($"Error while parsing ingredient: {name}, quantity: {quantity}, grams: {grams}");
            }

            var unit = RemoveUnitFromName(ref name);
            if (IngridientShouldBeRemoved(name))
            {
                continue;
            }

            ingredients.Add(new(name, quantity, unit, grams));
        }

        return ingredients.ToArray();
    }

    private static string RemoveUnitFromName(ref string name)
    {
        if (name.Contains("sztuk"))
        {
            name = name.Replace("sztuka", "").Replace("sztuki", "").Replace("sztuk", "").Trim();
            return "sztuka";
        }

        if (name.Contains("łyżecz"))
        {
            name = name.Replace("łyżeczka", "").Replace("łyżeczki", "").Replace("łyżeczek", "").Trim();
            return "łyżeczka";
        }

        if (name.Contains("łyż"))
        {
            name = name.Replace("łyżka", "").Replace("łyżki", "").Replace("łyżek", "").Trim();
            return "łyżka";
        }

        if (name.Contains("ząb"))
        {
            name = name.Replace("ząbek", "").Replace("ząbki", "").Replace("ząbków", "").Trim();
            return "ząbek";
        }

        if (name.Contains("garś"))
        {
            name = name.Replace("garść", "").Replace("garście", "").Replace("garście", "").Trim();
            return "garść";
        }

        if (name.Contains("szklan"))
        {
            name = name.Replace("szklanka", "").Replace("szklanki", "").Replace("szklanek", "").Trim();
            return "szklanka";
        }

        if (name.Contains("krom"))
        {
            name = name.Replace("kromka", "").Replace("kromki", "").Replace("kromek", "").Trim();
            return "kromka";
        }

        if (name.Contains("łodyg"))
        {
            name = name.Replace("łodyga", "").Replace("łodyg", "").Replace("łodygi", "").Trim();
            return "łodyga";
        }

        if (name.Contains("opak"))
        {
            name = name.Replace("opakowanie", "").Replace("opakowania", "").Replace("opakowań", "").Trim();
            return "opakowanie";
        }

        if (name.Contains("szczypt"))
        {
            name = name.Replace("szczypta", "").Replace("szczypty", "").Replace("szczypt", "").Trim();
            return "szczypta";
        }

        if (name.Contains("porcji"))
        {
            name = name.Replace("porcja", "").Replace("porcje", "").Replace("porcji", "").Trim();
            return "porcja";
        }

        if (name.Contains("plast"))
        {
            name = name.Replace("plaster", "").Replace("plasterów", "").Replace("plastery", "").Trim();
            return "plaster";
        }

        if (name.Contains("filiżan"))
        {
            name = name.Replace("filiżanka", "").Replace("filiżanek", "").Replace("filiżanki", "").Trim();
            return "filiżanka";
        }

        if (name.Contains("porcj"))
        {
            name = name.Replace("porcja", "").Replace("porcje", "").Replace("porcji", "").Trim();
            return "porcja";
        }

        if (name.Contains("list"))
        {
            name = name.Replace("listek", "").Replace("listki", "").Replace("listków", "").Trim();
            return "listki";
        }

        if (name.Contains("kielisz"))
        {
            name = name.Replace("kieliszek", "").Replace("kieliszka", "").Replace("kieliszków", "").Trim();
            return "kieliszek";
        }

        return string.Empty;
    }

    private static bool IngridientShouldBeRemoved(string name) =>
        name == "wody" || name.Contains("soli") || name.Contains("sól") || name == "pieprzu" || name.Contains("pieprzu czarnego");

    private static HtmlDocument GetDocument(string filePath)
    {
        var content = File.ReadAllText(filePath);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);
        return htmlDocument;
    }

    private static string GetDifficultyInfo(HtmlDocument htmlDocument) =>
        htmlDocument.DocumentNode.SelectSingleNode("//div[@class='prepare-info difficulty-info']//div[@class='prepare-info__title']").InnerHtml;

    private static string GetTotalPrepareTime(HtmlDocument htmlDocument) =>
        htmlDocument.DocumentNode.SelectSingleNode("//div[@class='prepare-info total-time']//div[@class='prepare-info__title']").InnerHtml;

    private static string GetPrepareTime(HtmlDocument htmlDocument) =>
        htmlDocument.DocumentNode.SelectSingleNode("//div[@class='prepare-info prepare-time']//div[@class='prepare-info__title']").InnerHtml;

    private static int GetPortionAmount(HtmlDocument htmlDocument) =>
        htmlDocument.DocumentNode.SelectSingleNode("//input[@class='recipe-calc-widget__input']").GetAttributeValue("value", 0);
}