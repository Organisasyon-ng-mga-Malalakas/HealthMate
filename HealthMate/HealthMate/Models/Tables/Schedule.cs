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