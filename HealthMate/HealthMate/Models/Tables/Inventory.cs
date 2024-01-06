using HealthMate.Converters;
using HealthMate.Enums;
using MongoDB.Bson;
using Realms;
using System.Text.Json.Serialization;

namespace HealthMate.Models.Tables;

public partial class Inventory : IRealmObject
{
	[PrimaryKey]
	[JsonPropertyName("inventory_id")]
	[JsonConverter(typeof(StringToObjectIdConverter))]
	public ObjectId InventoryId { get; set; }
	[JsonPropertyName("brand_name")]
	public string BrandName { get; set; }
	[JsonPropertyName("medicine_name")]
	public string MedicineName { get; set; }
	[JsonPropertyName("dosage")]
	public double Dosage { get; set; }
	[JsonPropertyName("dosage_unit")]
	public int DosageUnit { get; set; }
	[JsonPropertyName("stock")]
	public double Stock { get; set; }
	[JsonPropertyName("medication_type")]
	public int MedicationType { get; set; }
	[JsonPropertyName("description")]
	public string Description { get; set; }
	[JsonIgnore]
	public IList<Schedule> Schedules { get; }
	[JsonIgnore]
	public bool IsDeleted { get; set; }

	[Ignored]
	public string ImagePath => ((MedicationType)MedicationType).ImagePath();
	[Ignored]
	[JsonIgnore]
	public Color InventoryColor => Color.FromArgb(MedicationType == 0 || MedicationType % 2 == 0 ? "F26CA7" : "89CFF0");
	[Ignored]
	public string MedicineDisplayName => $"{BrandName} ({MedicineName})";
}