using System.Collections.ObjectModel;

namespace HealthMate.Models;

public class InventoryGroup : ObservableCollection<Tables.Inventory>
{
    public string GroupName { get; private set; }

    public InventoryGroup(string groupName, ObservableCollection<Tables.Inventory> inventories) : base(inventories)
    {
        GroupName = groupName;
    }
}