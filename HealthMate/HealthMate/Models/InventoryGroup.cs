namespace HealthMate.Models;

public class InventoryGroup : List<Tables.Inventory>
{
    public string GroupName { get; private set; }

    public InventoryGroup(string groupName, List<Tables.Inventory> inventories) : base(inventories)
    {
        GroupName = groupName;
    }
}