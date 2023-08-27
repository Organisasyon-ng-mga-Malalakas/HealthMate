using HealthMate.Enums;
using SQLite;

namespace HealthMate.Models.Tables;

public class Inventory
{
    [PrimaryKey, NotNull, Indexed]
    public string InventoryId { get; set; }
    [Unique, Indexed]
    public string BrandName { get; set; }
    public string MedicineName { get; set; }
    public int Dosage { get; set; }
    public int DosageUnit { get; set; }
    public int Stock { get; set; }
    public int MedicationType { get; set; }
    public string Description { get; set; }
    [Ignore]
    public string ImagePath => ((Enums.MedicationType)MedicationType).ImagePath();
    [Ignore]
    public Color InventoryColor => Color.FromArgb(MedicationType == 0 || MedicationType % 2 == 0 ? "F26CA7" : "89CFF0");
}