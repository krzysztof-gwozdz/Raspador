namespace Raspador.Scrapers.Respo.Models;

public record Ingredient(string Name, double Quantity, string Unit, double? Grams)
{
    public override string ToString() => $"{Grams}g {Name})";
}