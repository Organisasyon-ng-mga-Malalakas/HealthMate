using HealthMate.Enums;
using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;

public class Schedule
{
    [PrimaryKey]
    public ObjectId ScheduleId { get; set; }
    public Inventory Inventory { get; set; }
    public ScheduleState State { get; set; }
    public DateTime TimeToTake { get; set; }

    [Ignored]
    public string Icon => State.GetIcon();
    [Ignored]
    public string IconFontFamily => State.GetIconFontFamily();
    [Ignored]
    public string MedicineIcon => Inventory.ImagePath;
    [Ignored]
    public Color OverallColor => State.GetOverallColor();
}