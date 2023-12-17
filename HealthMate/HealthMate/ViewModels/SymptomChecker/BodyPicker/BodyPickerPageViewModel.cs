using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker;
public partial class BodyPickerPageViewModel(HttpService httpService, NavigationService navigationService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private string bodyPartImage = "empty_body_part";

	[ObservableProperty]
	private ObservableCollection<string> bodyParts;

	[ObservableProperty]
	private ObservableCollection<Issues> bodySublocations;

	[ObservableProperty]
	private double canCheckIllnessOpacity = 0.5;

	[ObservableProperty]
	[Required]
	private string? selectedBodyPart;

	[ObservableProperty]
	[Required]
	private Issues? selectedSubLocation;

	protected override void Initialization()
	{
		BodyParts = new ObservableCollection<string>(Enum.GetValues<BodyPart>()
		   .Select(_ => _.GetStringRepresentation()));
	}

	[RelayCommand]
	private async Task GotoIlnessCheckerPage()
	{
		if (HasErrors)
			return;

		await NavigationService.PushAsync(nameof(IllnessCheckerPage), new Dictionary<string, object>
		{
			{ "subLocation", SelectedSubLocation.Id }
		});
	}

	partial void OnSelectedBodyPartChanged(string value)
	{
		CanCheckIllnessOpacity = string.IsNullOrWhiteSpace(value) ? 0.5 : 1;
		BodyPartImage = value.GetBodyPartEnum().GetPhotoFileName();
		var bodyPart = value.GetBodyPartEnum();
		BodySublocations = new ObservableCollection<Issues>(httpService.SublocationsDictionary[bodyPart]);
	}

	partial void OnSelectedSubLocationChanged(Issues value)
	{
		CanCheckIllnessOpacity = SelectedSubLocation == null ? 0.5 : 1;
	}

	[RelayCommand]
	private async Task PopPage()
	{
		await NavigationService.PopAsync();
	}
}
