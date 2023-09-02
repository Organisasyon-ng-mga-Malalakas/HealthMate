﻿using HealthMate.Enums;
using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;

public partial class Schedule : IRealmObject
{
    [PrimaryKey]
    public ObjectId ScheduleId { get; set; }
    public Inventory Inventory { get; set; }
    public int ScheduleState { get; set; }
    public DateTimeOffset TimeToTake { get; set; }
    public string? Notes { get; set; }
    public double Quantity { get; set; }

    [Ignored]
    public string Icon => ((ScheduleState)ScheduleState).GetIcon();
    [Ignored]
    public string MedicineIcon => Inventory.ImagePath;
    [Ignored]
    public Color OverallColor => ((ScheduleState)ScheduleState).GetOverallColor();
}