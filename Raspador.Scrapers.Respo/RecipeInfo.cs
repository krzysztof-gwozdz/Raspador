using CsvHelper.Configuration;
using Raspador.Scrapers.Respo.Models;

namespace Raspador.Scrapers.Respo;

public record RecipeInfo(
    int Id,
    string Name,
    int PortionAmount,
    string PrepareTime,
    string TotalPrepareTime,
    string DifficultyInfo,
    int Calories,
    int Protein,
    int Fat,
    int Carbohydrates,
    int Sodium,
    int Fiber,
    string Ingredients)
{
    public RecipeInfo(Recipe recipe) : this(
        recipe.Id,
        recipe.Name,
        recipe.PortionAmount,
        recipe.PrepareTime,
        recipe.TotalPrepareTime,
        recipe.DifficultyInfo,
        recipe.Nutritions.Calories,
        recipe.Nutritions.Protein,
        recipe.Nutritions.Fat,
        recipe.Nutritions.Carbohydrates,
        recipe.Nutritions.Sodium,
        recipe.Nutritions.Fiber,
        string.Join(" | ", recipe.Ingredients.Select(ingredient => ingredient.ToString())))
    {
    }
}

public sealed class RecipeInfoMap : ClassMap<RecipeInfo>
{
    public RecipeInfoMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.Name).Name("Name");
        Map(m => m.PortionAmount).Name("Liczba porcji");
        Map(m => m.PrepareTime).Name("Czas przygotowania");
        Map(m => m.TotalPrepareTime).Name("Czas przygotowania (razem)");
        Map(m => m.DifficultyInfo).Name("Poziom trudności");
        Map(m => m.Calories).Name("Kalorie");
        Map(m => m.Protein).Name("Białko");
        Map(m => m.Fat).Name("Tłuszcz");
        Map(m => m.Carbohydrates).Name("Węglowodany");
        Map(m => m.Sodium).Name("Sód");
        Map(m => m.Fiber).Name("Błonnik");
        Map(m => m.Ingredients).Name("Składniki");
    }
}