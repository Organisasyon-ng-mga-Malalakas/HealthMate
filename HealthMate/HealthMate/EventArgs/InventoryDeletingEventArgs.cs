using MongoDB.Bson;

namespace HealthMate.EventArgs;
public class InventoryDeletingEventArgs : System.EventArgs
{
    public ObjectId InventoryId { get; }
    public string MedicationType { get; }

    public InventoryDeletingEventArgs(ObjectId inventoryId, string medicationType)
    {
        InventoryId = inventoryId;
        MedicationType = medicationType;
    }
}