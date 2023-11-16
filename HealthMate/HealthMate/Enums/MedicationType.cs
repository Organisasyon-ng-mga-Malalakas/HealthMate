namespace HealthMate.Enums;

public enum MedicationType
{
    Capsule,
    Drops,
    Inhaler,
    Injection,
    Liquid,
    Ointment,
    Other,
    Patch,
    Spray,
    Tablet
}

public static partial class Extensions
{
    public static IEnumerable<Dosage> AllowedDosages(this MedicationType medicationType)
    {
        return medicationType switch
        {
            MedicationType.Capsule or MedicationType.Tablet or MedicationType.Ointment => new Dosage[2] { Dosage.Milligrams, Dosage.Grams },
            MedicationType.Liquid => new Dosage[4] { Dosage.Teaspoons, Dosage.Tablespoons, Dosage.Milliliters, Dosage.Ounces },
            MedicationType.Inhaler => new Dosage[1] { Dosage.Puffs },
            MedicationType.Injection => new Dosage[2] { Dosage.Units, Dosage.Milliliters },
            MedicationType.Drops => new Dosage[2] { Dosage.Drops, Dosage.Milliliters },
            MedicationType.Spray => new Dosage[1] { Dosage.Sprays },
            MedicationType.Patch => new Dosage[1] { Dosage.Patches },
            MedicationType.Other => Enum.GetValues(typeof(Dosage)).Cast<Dosage>(),
            _ => Enumerable.Empty<Dosage>()
        };
    }

    public static string ImagePath(this MedicationType medicationType)
    {
        return $"{medicationType.ToString().ToLower()}.svg";
    }

    public static string GetDisplayUnit(this MedicationType medicationType)
    {
        return medicationType switch
        {
            MedicationType.Capsule => "capsule(s)",
            MedicationType.Drops => "drop(s)",
            MedicationType.Inhaler => "puff(s)",
            MedicationType.Injection => "injection(s)",
            MedicationType.Liquid => "milliliter(s)",
            MedicationType.Ointment => "gram(s)",
            MedicationType.Other => "units",
            MedicationType.Patch => "patche(s)",
            MedicationType.Spray => "spray(s)",
            MedicationType.Tablet => "tablet(s)",
            _ => "units", // Fallback, should not be needed
        };
    }
}