using HealthMate.Converters;
using HealthMate.Enums;
using HealthMate.Models.Tables;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace HealthMate.Models;

public class InventoryDTO
{
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
	public IList<Schedule> Schedules { get; set; }
	[JsonPropertyName("deleted_at")]
	[JsonConverter(typeof(DateTimeOffsetToDateTimeConverter))]
	public DateTime? DeletedAt { get; set; }

	[JsonIgnore]
	public string ImagePath => ((MedicationType)MedicationType).ImagePath();
	[JsonIgnore]
	public Color InventoryColor => Color.FromArgb(MedicationType == 0 || MedicationType % 2 == 0 ? "F26CA7" : "89CFF0");
	[JsonIgnore]
	public string MedicineDisplayName => $"{BrandName} ({MedicineName})";
}

public static class InventoryExtension
{
	public static Inventory ToInventory(this InventoryDTO inventory)
	{
		return new Inventory
		{
			BrandName = inventory.BrandName,
			Description = inventory.Description,
			Dosage = inventory.Dosage,
			DosageUnit = inventory.DosageUnit,
			InventoryId = inventory.InventoryId,
			DeletedAt = inventory.DeletedAt,
			MedicationType = inventory.MedicationType,
			MedicineName = inventory.MedicineName,
			Stock = inventory.Stock
		};
	}

	public static InventoryDTO ToDataTransferObject(this Inventory inventory)
	{
		return new InventoryDTO
		{
			BrandName = inventory.BrandName,
			Description = inventory.Description,
			Dosage = inventory.Dosage,
			DosageUnit = inventory.DosageUnit,
			InventoryId = inventory.InventoryId,
			DeletedAt = (inventory.DeletedAt ?? DateTime.Now).DateTime,
			MedicationType = inventory.MedicationType,
			MedicineName = inventory.MedicineName,
			Schedules = inventory.Schedules,
			Stock = inventory.Stock
		};
	}
}