using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;
public partial class IllnessInfoPopupViewModel(NavigationService navigationService,
	SymptomCheckerService httpService,
	PopupService popupService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private ObservableCollection<DiagnosisInfo> diagnosisInfo;

	[ObservableProperty]
	private string illnessName;

	[ObservableProperty]
	private bool isLoading;

	[ObservableProperty]
	private int issueId;

	[ObservableProperty]
	private Color textColors;

	[RelayCommand]
	private async Task ClosePopup()
	{
		await popupService.ClosePopup();
	}

	public override async void OnNavigatedTo()
	{
		IsLoading = true;
		var illnessInfo = await httpService.GetIssueInfo(IssueId);
		var isCritical = Random.Shared.Next(0, 2) == 1;
		TextColors = (Color)Application.Current.Resources[isCritical ? "Red" : "Blue"];
		var nonCriticalSecondTtlte = new string[10]
		{
			"Healing Paths 🌱",
			"Care Treatments 💊",
			"Better Health 🌟",
			"Recovery Journey 🛤️",
			"Heal Together 👭",
			"Wellness Plan 📋",
			"Therapy Options 💡",
			"Gentle Healing 🍃",
			"Treatment Allies 👩‍⚕️👨‍⚕️",
			"Soothing Remedies 🌼"
		};
		var criticalSecondTitles = new string[10]
		{
			"Doctor's Insight 🏥",
			"Healing Hands 🤲",
			"MediCare Experts 🩺",
			"Professional Aid 🚑",
			"Health Pros 🌟",
			"Care Advisors 💼",
			"Wellness Authorities 👩‍⚕️",
			"Clinical Support 🏨",
			"MediGuidance 🧭",
			"Doc's Corner 📚",
		};
		var criticalThirdTitles = new string[10]
		{
			"By Your Side 🤝",
			"Support Squad ❤️",
			"Care Partners 👫",
			"Health Allies 🤗",
			"Journey Mates 🛤️",
			"Wellness Companions 🍀",
			"Trust Circle 🔵",
			"Guiding Stars ✨",
			"Helping Hands 🤝",
			"Friendly Guide 🐾"
		};
		var randomNumberForTitle = Random.Shared.Next(10);
		var possibleSymptoms = string.Join(", ", illnessInfo.PossibleSymptoms.Split(',')
			.Select((s, index) => index == 0
				? $"{char.ToUpper(s.Trim()[0])}{s.Trim()[1..].ToLower()}"
				: s.Trim().ToLower()));

		DiagnosisInfo =
		[
			new DiagnosisInfo
			{
				Description = $"Possible symptoms: {possibleSymptoms}\n\n{illnessInfo.Description}",
				Name = illnessInfo.Name,
				Image = isCritical ? "critical0" : "noncritical0"
			},
			new DiagnosisInfo
			{
				Description = illnessInfo.TreatmentDescription,
				Name = (isCritical ? criticalSecondTitles : nonCriticalSecondTtlte)[randomNumberForTitle],
				Image = isCritical ? "critical1" : "noncritical1"
			},
			new DiagnosisInfo
			{
				Description = isCritical
					? "We're here to support you alongside medical professionals. You're not alone, and our focus is on your well-being and the best possible outcomes. Stay strong; we're here for you every step of the way."
					: $"While rest and over-the-counter meds help with {illnessInfo.Name.ToLower()} temporarily, consult a healthcare professional for personalized guidance and treatment if you're concerned about your health. Your well-being is our priority.",
				Name = "Things to remember",
				Image = isCritical ? "critical2" : "noncritical2"
			}
		];
		IsLoading = false;
	}
}