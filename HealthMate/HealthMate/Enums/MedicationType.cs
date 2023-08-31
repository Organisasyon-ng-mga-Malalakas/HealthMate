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
    public static string ImagePath(this MedicationType medicationType)
    {
        //return medicationType switch
        //{
        //    MedicationType.Capsule => throw new NotImplementedException(),
        //    MedicationType.Drops => throw new NotImplementedException(),
        //    MedicationType.Inhaler => throw new NotImplementedException(),
        //    MedicationType.Injection => throw new NotImplementedException(),
        //    MedicationType.Liquid => throw new NotImplementedException(),
        //    MedicationType.Ointment => throw new NotImplementedException(),
        //    MedicationType.Other => throw new NotImplementedException(),
        //    MedicationType.Patch => throw new NotImplementedException(),
        //    MedicationType.Spray => throw new NotImplementedException(),
        //    MedicationType.Tablet => throw new NotImplementedException(),
        //    _ => throw new NotImplementedException(),
        //};
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