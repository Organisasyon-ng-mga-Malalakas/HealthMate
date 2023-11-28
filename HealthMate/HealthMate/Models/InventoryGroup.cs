using HealthMate.Models.Tables;
using System.Collections.ObjectModel;

namespace HealthMate.Models;
public class InventoryGroup(string category, ObservableCollection<Inventory> inventory) : ObservableCollection<Inventory>(inventory)
{
	public string Category { get; set; } = category;
	public ObservableCollection<Inventory> Inventory { get; set; } = inventory;
}