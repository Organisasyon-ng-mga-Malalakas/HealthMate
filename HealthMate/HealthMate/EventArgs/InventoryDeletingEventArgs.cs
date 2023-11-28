using MongoDB.Bson;

namespace HealthMate.EventArgs;
public class InventoryDeletingEventArgs(ObjectId inventoryId, string medicationType) : System.EventArgs
{
	public ObjectId InventoryId { get; } = inventoryId;
	public string MedicationType { get; } = medicationType;
}