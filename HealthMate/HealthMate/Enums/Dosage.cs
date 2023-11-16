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
    Puffs,
    Sprays,
    Patches
}

public static partial class Extensions
{
    public static IEnumerable<MedicationType> AllowedMedicationTypes(this Dosage dosage)
    {
        return dosage switch
        {
            Dosage.Milligrams or Dosage.Grams => new MedicationType[2] { MedicationType.Capsule, MedicationType.Tablet },
            Dosage.Teaspoons or Dosage.Tablespoons or Dosage.Ounces => new MedicationType[1] { MedicationType.Liquid },
            Dosage.Puffs => new MedicationType[1] { MedicationType.Inhaler },
            Dosage.Units => new MedicationType[1] { MedicationType.Injection },
            Dosage.Drops => new MedicationType[1] { MedicationType.Drops },
            Dosage.Milliliters => new MedicationType[3] { MedicationType.Liquid, MedicationType.Injection, MedicationType.Drops },
            Dosage.Sprays => new MedicationType[1] { MedicationType.Spray },
            Dosage.Patches => new MedicationType[1] { MedicationType.Patch },
            _ => Enum.GetValues(typeof(MedicationType)).Cast<MedicationType>(),
        };
    }

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
            Dosage.Sprays => "spray(s)",
            Dosage.Patches => "patches",
            _ => "Unknown",
        };
    }
}