namespace Raspador.Scrapers.Respo.Models;

public record Nutritions(int Calories, int Protein, int Fat, int Carbohydrates, int Sodium, int Fiber)
{
    public override string ToString() => $"Kalorie: {Calories} kcal, Białko: {Protein}g, Tłuszcz: {Fat}g, Węglowodany: {Carbohydrates}g, Błonnik: {Fiber}g, Sód: {Sodium}mg";
}   