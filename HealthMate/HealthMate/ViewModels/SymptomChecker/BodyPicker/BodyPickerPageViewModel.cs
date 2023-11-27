using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Services.HttpServices.Symptoms;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker;
public partial class BodyPickerPageViewModel : BaseViewModel
{
    public BodyPickerPageViewModel(NavigationService navigationService) : base(navigationService)
    {

    }

    [ObservableProperty]
    private string bodyPartImage = "empty_body_part";

    [ObservableProperty]
    private ObservableCollection<string> bodyParts;

    [ObservableProperty]
    private string selectedBodyPart;

    [ObservableProperty]
    private double canCheckIllnessOpacity = 0.5;

    protected override void Initialization()
    {
        BodyParts = new ObservableCollection<string>(Enum.GetValues<BodyPart>()
           .Select(bodyPart =>
           {
               return bodyPart switch
               {
                   BodyPart.Head or BodyPart.Legs or BodyPart.Arms or BodyPart.General => bodyPart.ToString(),
                   BodyPart.Upperbody => "Upper body",
                   BodyPart.Lowerbody => "Lower body",
                   _ => "",
               };
           }));
    }

    [RelayCommand]
    private async Task GotoIlnessCheckerPage()
    {
        if (CanCheckIllnessOpacity < 1)
            return;

        var bodyPart = SelectedBodyPart switch
        {
            "Head" => BodyPart.Head,
            "Legs" => BodyPart.Legs,
            "Arms" => BodyPart.Arms,
            "General" => BodyPart.General,
            "Upper body" => BodyPart.Upperbody,
            "Lower body" => BodyPart.Lowerbody,
            _ => throw new ArgumentException("Illegal body part!"),
        };
        await Shell.Current.GoToAsync(nameof(IllnessCheckerPage), true, new Dictionary<string, object>
        {
            { "bodyPart", bodyPart }
        });
    }

    partial void OnSelectedBodyPartChanged(string value)
    {
        CanCheckIllnessOpacity = string.IsNullOrWhiteSpace(value) ? 0.5 : 1;
        BodyPartImage = value switch
        {
            "Head" or "Legs" or "Arms" or "General" => value.ToLower(),
            "Upper body" => "upperbody",
            "Lower body" => "lowerbody",
            _ => ""
        };
    }

    [RelayCommand]
    private async Task PopPage()
    {
        await NavigationService.PopAsync();
    }
}
