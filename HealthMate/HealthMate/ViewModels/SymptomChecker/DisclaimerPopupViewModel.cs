﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.SymptomChecker;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.SymptomChecker;

public partial class DisclaimerPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    [ObservableProperty]
    private ObservableCollection<Disclaimer> disclaimers;

    public DisclaimerPopupViewModel(PopupService popupService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    public async Task ClosePopup()
    {
        await _popupService.ClosePopup();
        await Shell.Current.GoToAsync($"{nameof(DiseaseCheckerPage)}", true);
    }

    public override void OnNavigatedTo()
    {
        var titles = new string[4] { "Informational purposes only", "User-input accuracy", "Seek medical attention", "Prioritize your well-being" };
        var subtitles = new string[4]
        {
            "Symptom Checker is solely based on the user's input and should be used for general reference We are not medical professionals, and any assumptions made about the symptoms are not a substitute for professional medical advice.",
            "HealthMate's responses are generated by algorithms and do not constitute a medical diagnosis. Misinformation or inaccuracies in the user input may lead to incorrect results.",
            "If you have any underlying health conditions, or are uncertain about the results, we strongly recommend seeking advice from a medical professional. Your health is our utmost priority, and consulting a healthcare provider can provide you with accurate and personalized guidance for your specific health needs.",
            "Make informed decisions regarding your health. If you have any doubts or concerns, do not hesitate to seek professional medical assistance. Thank you for using HealthMate, and we wish you good health and well-being."
        };

        Disclaimers = [];
        for (var index = 0; index < 4; index++)
        {
            Disclaimers.Add(new Disclaimer
            {
                ImagePath = $"disclaimer{index}.svg",
                Subtitle = subtitles[index],
                Title = titles[index]
            });
        }
    }
}
