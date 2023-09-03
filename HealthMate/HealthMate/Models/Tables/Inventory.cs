using HealthMate.Enums;
using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;

public partial class Inventory : IRealmObject
{
    [PrimaryKey]
    public ObjectId InventoryId { get; set; }
    public string BrandName { get; set; }
    public string MedicineName { get; set; }
    public double Dosage { get; set; }
    public int DosageUnit { get; set; }
    public double Stock { get; set; }
    public int MedicationType { get; set; }
    public string Description { get; set; }
    public IList<Schedule> Schedules { get; }

    [Ignored]
    public string ImagePath => ((MedicationType)MedicationType).ImagePath();
    [Ignored]
    public Color InventoryColor => Color.FromArgb(MedicationType == 0 || MedicationType % 2 == 0 ? "F26CA7" : "89CFF0");
    [Ignored]
    public string MedicineDisplayName => $"{BrandName} ({MedicineName})";
}