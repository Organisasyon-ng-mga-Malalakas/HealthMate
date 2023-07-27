using HealthMate.Services;
using HealthMate.ViewModels;
using HealthMate.Views;
using Mopups.Interfaces;
using Mopups.Services;

namespace HealthMate;

internal static class PrismStartup
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.RegisterTypes(RegisterTypes)
            .OnAppStart($"NavigationPage/{nameof(GetStartedPage)}");
    }

    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry
            // Services
            .RegisterSingleton<IPopupNavigation, PopupNavigation>()
            .RegisterSingleton<PopupService>()

            // Pages
            .RegisterForNavigation<MainPage, MainPageViewModel>()
            .RegisterForNavigation<GetStartedPage, GetStartedPageViewModel>()
            .RegisterForNavigation<OnboardingPage, OnboardingPageViewModel>()
            .RegisterForNavigation<TermsAndConditionPopup, TermsAndConditionPopupViewModel>()
            .RegisterInstance(SemanticScreenReader.Default);
    }
}
