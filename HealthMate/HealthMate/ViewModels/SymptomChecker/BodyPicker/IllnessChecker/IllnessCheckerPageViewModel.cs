using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Templates;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;
public partial class IllnessCheckerPageViewModel(NavigationService navigationService,
	PopupService popupService,
	SymptomCheckerService symptomCheckerService) : BaseViewModel(navigationService)
{
	private IEnumerable<SymptomInfo> _symptoms;

	[ObservableProperty]
	private ObservableCollection<Diagnosis> illnesses;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(FindIllnessCommand))]
	private bool isLoading;

	[ObservableProperty]
	private string searchTerm;

	[ObservableProperty]
	private int subLocationId;

	[ObservableProperty]
	private SortableObservableCollection<SymptomInfo> symptoms;

	[RelayCommand(CanExecute = nameof(CanFindIllness))]
	private async Task FindIllness()
	{
		IsLoading = true;
		if (Illnesses != null && Illnesses.Count > 0)
			Illnesses.Clear();

		var symptomIds = Symptoms.Where(_ => _.IsSelected)
			.Select(_ => _.Id);

		var diagnosis = await symptomCheckerService.GetDiagnosis(symptomIds);
		Illnesses = new ObservableCollection<Diagnosis>(diagnosis);
		IsLoading = false;
	}

	[RelayCommand]
	private void SearchTermChanged()
	{
		if (string.IsNullOrWhiteSpace(SearchTerm))
		{
			foreach (var item in _symptoms)
				if (!Symptoms.Contains(item))
					Symptoms.Add(item);

			Symptoms.Sort(_ => _.Name);
		}
	}

	[RelayCommand]
	private void SearchSymptom()
	{
		if (string.IsNullOrWhiteSpace(SearchTerm))
		{
			foreach (var item in _symptoms)
				if (!Symptoms.Contains(item))
					Symptoms.Add(item);

			Symptoms.Sort(_ => _.Name);
		}

		var filteredSymptoms = _symptoms.Where(_ => _.Name.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase));
		foreach (var item in _symptoms.ToList())
		{
			if (!filteredSymptoms.Contains(item))
				Symptoms.Remove(item);
			else if (!Symptoms.Contains(item))
				Symptoms.Add(item);

			Symptoms.Sort(_ => _.Name);
		}
	}

	private bool CanFindIllness()
	{
		return !IsLoading;
	}

	[RelayCommand]
	private async Task GetIllnessInfo(Diagnosis diagnosis)
	{
		await popupService.ShowPopup<IllnessInfoPopup>(diagnosis.Issue.Id);
	}

	public override async void OnNavigatedTo()
	{
		IsLoading = true;
		_symptoms = await symptomCheckerService.GetSymptoms(SubLocationId);
		Symptoms = new SortableObservableCollection<SymptomInfo>(_symptoms.OrderBy(_ => _.Name));
		IsLoading = false;
	}

	protected override void ReceiveParameters(IDictionary<string, object> query)
	{
		SubLocationId = (int)query["subLocation"];
	}
}