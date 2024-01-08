using HealthMate.Converters;
using HealthMate.Models.Tables;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace HealthMate.Models;
public class ScheduleDTO
{
	[JsonPropertyName("schedule_id")]
	[JsonConverter(typeof(StringToObjectIdConverter))]
	public ObjectId ScheduleId { get; set; }
	[JsonIgnore]
	public Inventory Inventory { get; set; }
	[JsonPropertyName("inventory_id")]
	public string InventoryId { get; set; }
	[JsonPropertyName("schedule_state")]
	public int ScheduleState { get; set; }
	[JsonPropertyName("time_to_take")]
	public DateTimeOffset TimeToTake { get; set; }
	[JsonPropertyName("notes")]
	public string? Notes { get; set; }
	[JsonPropertyName("quantity")]
	public double Quantity { get; set; }
	[JsonPropertyName("image")]
	public string PhotoBase64 { get; set; }
	[JsonPropertyName("updated_at")]
	[JsonConverter(typeof(DateTimeOffsetToDateTimeConverter))]
	public DateTime? UpdatedAt { get; set; }
}

public static class ScheduleExtension
{
	public static Schedule ToSchedule(this ScheduleDTO schedule)
	{
		return new Schedule
		{
			Notes = schedule.Notes,
			PhotoBase64 = schedule.PhotoBase64,
			Quantity = schedule.Quantity,
			ScheduleId = schedule.ScheduleId,
			ScheduleState = schedule.ScheduleState,
			TimeToTake = schedule.TimeToTake,
			UpdatedAt = schedule.UpdatedAt
		};
	}

	public static ScheduleDTO ToDataTransferObject(this Schedule schedule)
	{
		return new ScheduleDTO
		{
			Inventory = schedule.Inventory,
			InventoryId = schedule.Inventory.InventoryId.ToString(),
			Notes = schedule.Notes,
			PhotoBase64 = schedule.PhotoBase64,
			Quantity = schedule.Quantity,
			ScheduleId = schedule.ScheduleId,
			ScheduleState = schedule.ScheduleState,
			TimeToTake = schedule.TimeToTake.AddHours(8),
			UpdatedAt = (schedule.UpdatedAt ?? DateTime.Now).DateTime
		};
	}
}