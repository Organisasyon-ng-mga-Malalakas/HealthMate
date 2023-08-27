namespace HealthMate.Enums;

public enum Dosage
{
    Milligrams,
    Grams,
    Teaspoons,
    Tablespoons,
    Units,
    Drops,
    Milliliters,
    Ounces,
    Puffs
}

public static partial class Extensions
{
    public static string GetAcronym(this Dosage dosage)
    {
        return dosage switch
        {
            Dosage.Milligrams => "mg",
            Dosage.Grams => "g",
            Dosage.Teaspoons => "tsp",
            Dosage.Tablespoons => "tbsp",
            Dosage.Units => "U",
            Dosage.Drops => "dr",
            Dosage.Milliliters => "ml",
            Dosage.Ounces => "oz",
            Dosage.Puffs => "puff(s)",
            _ => "Unknown",
        };
    }
}