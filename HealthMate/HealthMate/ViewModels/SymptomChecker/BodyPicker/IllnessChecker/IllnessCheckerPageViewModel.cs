using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using HealthMateBackend;
using System.Collections.ObjectModel;
using BodyPart = HealthMateBackend.Body_part;

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
    private ObservableCollection<Diagnosis> illnesses;

    [ObservableProperty]
    private ObservableCollection<SimilarSymptom> similarIllnesses;

    [ObservableProperty]
    private ObservableCollection<Symptoms> symptoms;

    [RelayCommand]
    private async Task FindIllness()
    {
        var symptomIds = string.Join(",",
            Symptoms.Where(_ => _.IsSelected)
            .Select(symptom => symptom.Id.ToString()));

        var diagnosis = await _httpService.GetDiseaseFromSymptoms(2001, Gender.Male, BodyPart, symptomIds);
        Illnesses = new ObservableCollection<Diagnosis>(diagnosis.Diagnosis);
        SimilarIllnesses = new ObservableCollection<SimilarSymptom>(diagnosis.SimilarSymptoms);
    }

    [RelayCommand]
    private async Task GetIllnessInfo()
    {
        await _popupService.ShowPopup<IllnessInfoPopup>();
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