using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;

public partial class Schedule : IRealmObject, IEquatable<Schedule>
{
	[PrimaryKey]
	public ObjectId ScheduleId { get; set; }
	public Inventory Inventory { get; set; }
	public int ScheduleState { get; set; }
	public DateTimeOffset TimeToTake { get; set; }
	public string? Notes { get; set; }
	public double Quantity { get; set; }
	public string PhotoBase64 { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }

	[Ignored]
	public string MedicineIcon => Inventory == null ? "" : Inventory.ImagePath;

	public bool Equals(Schedule other)
	{
		return TimeToTake.Equals(other.TimeToTake);
	}

	public override int GetHashCode()
	{
		return TimeToTake.GetHashCode();
	}
}

//public class ScheduleRemote(Schedule schedule) : ObservableObject
//{
//	[JsonPropertyName("schedule_id")]
//	public string ScheduleId { get; } = schedule.ScheduleId.ToString();
//	[JsonPropertyName("schedule_state")]
//	public int ScheduleState { get; } = schedule.ScheduleState;
//	[JsonPropertyName("time_to_take")]
//	public DateTime TimeToTake { get; } = schedule.TimeToTake.DateTime;
//	[JsonPropertyName("notes")]
//	public string Notes { get; } = schedule.Notes;
//	[JsonPropertyName("quantity")]
//	public double Quantity { get; } = schedule.Quantity;
//	[JsonPropertyName("image")]
//	public string PhotoBase64 { get; } = schedule.PhotoBase64;
//}

//public class ScheduleRemote : ObservableObject
//{
//	public ScheduleRemote() { }

//	public ScheduleRemote(Schedule schedule)
//	{
//		ScheduleId = schedule.ScheduleId.ToString();
//		ScheduleState = schedule.ScheduleState;
//		TimeToTake = schedule.TimeToTake.DateTime;
//		Notes = schedule.Notes;
//		Quantity = schedule.Quantity;
//		PhotoBase64 = schedule.PhotoBase64;
//	}

//	[JsonPropertyName("schedule_id")]
//	public string ScheduleId { get; }
//	[JsonPropertyName("schedule_state")]
//	public int ScheduleState { get; }
//	[JsonPropertyName("time_to_take")]
//	public DateTime TimeToTake { get; }
//	[JsonPropertyName("notes")]
//	public string Notes { get; }
//	[JsonPropertyName("quantity")]
//	public double Quantity { get; }
//	[JsonPropertyName("image")]
//	public string PhotoBase64 { get; }
//}