using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using Realms;
using System.Text.Json.Serialization;

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

	[Ignored]
	public string MedicineIcon => Inventory.ImagePath;

	public bool Equals(Schedule other)
	{
		return TimeToTake.Equals(other.TimeToTake);
	}

	public override int GetHashCode()
	{
		return TimeToTake.GetHashCode();
	}
}

public class ScheduleRemote(Schedule schedule) : ObservableObject
{
	[JsonPropertyName("schedule_id")]
	public string ScheduleId { get; } = schedule.ScheduleId.ToString();
	[JsonPropertyName("schedule_state")]
	public int ScheduleState { get; } = schedule.ScheduleState;
	[JsonPropertyName("time_to_take")]
	public DateTime TimeToTake { get; } = schedule.TimeToTake.DateTime;
	[JsonPropertyName("notes")]
	public string Notes { get; } = schedule.Notes;
	[JsonPropertyName("quantity")]
	public double Quantity { get; } = schedule.Quantity;
	[JsonPropertyName("image")]
	public string PhotoBase64 { get; } = schedule.PhotoBase64;
}