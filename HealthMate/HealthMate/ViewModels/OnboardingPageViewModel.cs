using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Constants;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels;

public partial class OnboardingPageViewModel : BaseViewModel
{
    private PopupService _popupService;

    public OnboardingPageViewModel(INavigationService navigationService, PopupService popupService) : base(navigationService)
    {
        _popupService = popupService;
    }

    [ObservableProperty]
    private string checkBtnText = FontAwesomeIcons.ChevronRight;
    [ObservableProperty]
    private ObservableCollection<Onboarding> onboarding;
    [ObservableProperty]
    private int position;

    [RelayCommand]
    private void MoveBackward()
    {
        if (Position > 0)
            Position--;
    }

    [RelayCommand]
    private async Task MoveForward()
    {
        if (Position != 3)
            Position++;
        else
            await _popupService.ShowPopup<TermsAndConditionPopup>();
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        var title1 = new string[4] { "YOUR PERSONAL", "TRACK AND MANAGE", "DISCOVER AND EXPLORE", "HEALTHMATE" };
        var title2 = new string[4] { "MEDICATION COMPANION", "YOUR MEDICATION INVENTORY", "MEDICATIONS", "SYMPTOM CHECKER" };
        var subtitle = new string[4]
        {
            "Stay organized and never miss a medication dose with HealthMate. Our user-friendly app provides personalized reminders, ensuring you stay on top of your meidcation schedule effortlessly.",
            "Effortlessly monitor and manage your medication inventory with HealthMate's Medication Inventory feature. Stay informed about your available medication, and ensire you never run out of essential medications.",
            "Explore an extensive collection of medications with HealthMate's Browse Medicine feature. Discover vital information, including medicine names, categories, descriptions, side effects and popular brands. Easily find the medications you need and gain valuable insights for informed decision-making about your healthcare.",
            "Take control of your health with HealthMate Symptom Checker. Our comprehensive tool helps you understand and manage your symptoms effectively. From common ailments to potential underlying conditions, HealthMAte provides reliable information and guidance to empower you in your health journey. Stay informed and make informed decisions with HealthMate Symptom Checker."
        };

        Onboarding = new ObservableCollection<Onboarding>();
        for (var index = 0; index < 4; index++)
            Onboarding.Add(new Onboarding
            {
                Image = $"onboarding{index + 1}.png",
                Subtitle = subtitle[index],
                Title1 = title1[index],
                Title2 = title2[index]
            });
    }

    partial void OnPositionChanged(int oldValue, int newValue)
    {
        CheckBtnText = newValue == 3 ? FontAwesomeIcons.Check : FontAwesomeIcons.ChevronRight;
    }
}
