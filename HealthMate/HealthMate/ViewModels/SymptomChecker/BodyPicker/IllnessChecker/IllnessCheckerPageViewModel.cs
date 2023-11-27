using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Services.HttpServices.Symptoms;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using System.Collections.ObjectModel;
using BodyPart = HealthMate.Services.HttpServices.Symptoms.BodyPart;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;
public partial class IllnessCheckerPageViewModel : BaseViewModel
{
    private readonly HttpService _httpService;
    private readonly PopupService _popupService;

    public IllnessCheckerPageViewModel(NavigationService navigationService,
        HttpService httpService,
        PopupService popupService) : base(navigationService)
    {
        _httpService = httpService;
        _popupService = popupService;
    }

    [ObservableProperty]
    private BodyPart bodyPart;

    [ObservableProperty]
    private ObservableCollection<DiagnosisGroup> illnesses;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FindIllnessCommand))]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<Symptoms> symptoms;

    [RelayCommand(CanExecute = nameof(CanFindIllness))]
    private async Task FindIllness()
    {
        IsLoading = true;

        if (Illnesses != null && Illnesses.Count > 0)
            Illnesses.Clear();

        var symptomIds = string.Join(",",
            Symptoms.Where(_ => _.IsSelected)
            .Select(symptom => symptom.Id.ToString()));

        var diagnosis = await _httpService.GetDiseaseFromSymptoms(2001, BodyPart, Gender.Male, symptomIds);
        Illnesses =
        [
            new DiagnosisGroup(diagnosis.Diagnosis.Count() > 1 ? "Possible illnesses" : "Possible illness", diagnosis.Diagnosis.ToList()),
            new DiagnosisGroup(diagnosis.SimilarIllness.Count() > 1 ? "Similar illnesses" : "Similar illness", diagnosis.SimilarIllness.ToList()),
        ];

        IsLoading = false;
    }

    private bool CanFindIllness()
    {
        return !IsLoading;
    }

    [RelayCommand]
    private async Task GetIllnessInfo(Diagnosis diagnosis)
    {
        await _popupService.ShowPopup<IllnessInfoPopup>(diagnosis, BodyPart);
    }

    public override void OnNavigatedTo()
    {
        Symptoms = new ObservableCollection<Symptoms>(_httpService.GetSymptoms(BodyPart));
    }

    protected override void ReceiveParameters(IDictionary<string, object> query)
    {
        BodyPart = (BodyPart)query["bodyPart"];
    }
}