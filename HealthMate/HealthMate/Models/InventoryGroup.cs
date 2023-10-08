using HealthMate.Models.Tables;
using System.Collections.ObjectModel;

namespace HealthMate.Models;
public class InventoryGroup : ObservableCollection<Inventory>
{
    public string Category { get; set; }
    public ObservableCollection<Inventory> Inventory { get; set; }

    public InventoryGroup(string category, ObservableCollection<Inventory> inventory) : base(inventory)
    {
        Category = category;
        Inventory = inventory;
    }
}