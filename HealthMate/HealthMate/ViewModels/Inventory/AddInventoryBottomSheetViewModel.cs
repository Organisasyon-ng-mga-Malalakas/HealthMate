using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Platforms.Android.Services;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;

public partial class AddInventoryBottomSheetViewModel(BottomSheetService bottomSheetService,
	InventoryService inventoryService,
	KeyboardService keyboardService,
	NavigationService navigationService,
	IPreferences preferences) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	[Required]
	private string brandName;

	[ObservableProperty]
	private string description;

	[ObservableProperty]
	[Required]
	private double dosage;

	[ObservableProperty]
	private ObservableCollection<Dosage> dosages;

	[ObservableProperty]
	private bool isMedicationTypePickerEnabled;

	[ObservableProperty]
	private ObservableCollection<MedicationType> medicationTypes;

	[ObservableProperty]
	[Required]
	private string medicineName;

	[ObservableProperty]
	[Required]
	private Dosage? selectedDosage = null;

	[ObservableProperty]
	[Required]
	private MedicationType? selectedmedicationType = null;

	[ObservableProperty]
	[Required]
	private double stock;

	[RelayCommand]
	private async Task AddInventory()
	{
		ValidateAllProperties();
		if (HasErrors)
			return;

		//var faker = new Faker<InventoryTable>()
		//	.RuleFor(p => p.InventoryId, _ => ObjectId.GenerateNewId())
		//	.RuleFor(p => p.BrandName, v => v.Internet.UserName())
		//	.RuleFor(p => p.Description, v => v.Lorem.Word())
		//	.RuleFor(p => p.DosageUnit, v => v.Random.Int())
		//	.RuleFor(p => p.IsDeleted, _ => false)
		//	.RuleFor(p => p.MedicationType, v => v.Random.Int())
		//	.RuleFor(p => p.MedicineName, v => v.Internet.UserName())
		//	.RuleFor(p => p.Stock, v => v.Random.Double())
		//	.Generate(1)[0];
		//await inventoryService.UpsertInventory(new List<InventoryTable> { faker });

		await inventoryService.UpsertInventory(new List<InventoryTable>
		{
			new() {
				InventoryId = ObjectId.GenerateNewId(),
				BrandName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(BrandName).Trim(),
				Description = Description,
				Dosage = Dosage,
				DosageUnit = (int)SelectedDosage,
				IsDeleted = false,
				MedicationType = (int)SelectedmedicationType,
				MedicineName = MedicineName,
				Stock = Stock
			}
		});

		await CloseBottomSheet();
	}

	[RelayCommand]
	private async Task CloseBottomSheet()
	{
		keyboardService.HideKeyboard();
		await bottomSheetService.CloseBottomSheet();
	}

	public override void OnNavigatedTo()
	{
		Dosages = new ObservableCollection<Dosage>(Enum.GetValues<Dosage>());
		//MedicationTypes = new ObservableCollection<MedicationType>(Enum.GetValues<MedicationType>());
	}

	partial void OnSelectedDosageChanged(Dosage? value)
	{
		if (value == null)
			return;
		IsMedicationTypePickerEnabled = value != null;
		MedicationTypes = new ObservableCollection<MedicationType>(value.Value.AllowedMedicationTypes());
	}
}