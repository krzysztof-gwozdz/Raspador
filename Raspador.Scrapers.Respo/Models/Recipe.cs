namespace Raspador.Scrapers.Respo.Models;

public record Recipe(
    int Id,
    string Name,
    int PortionAmount,
    string PrepareTime,
    string TotalPrepareTime,
    string DifficultyInfo,
    Nutritions Nutritions,
    Ingredient[] Ingredients)

{
    public override string ToString() => $"{Id} - {Name} [{PortionAmount}] - {PrepareTime} - {TotalPrepareTime} - {DifficultyInfo} " +
                                         $"\nWartości odżywcze: {Nutritions}" +
                                         $"\nSkładniki:\n{string.Join("\n", Ingredients.Select(i => i.ToString()))}";
}