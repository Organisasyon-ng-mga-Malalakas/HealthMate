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
        var navigationPath = VersionTracking.IsFirstLaunchEver
            ? $"{nameof(NavigationPage)}/{nameof(GetStartedPage)}"
            : $"{nameof(NavigationPage)}/{nameof(HomePage)}";

        builder.RegisterTypes(RegisterTypes)
            .OnAppStart(navigationPath);
    }

    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry
            // Services
            .RegisterSingleton<IPopupNavigation, PopupNavigation>()
            .RegisterSingleton<PopupService>()
            .RegisterSingleton<IVersionTracking>(_ => VersionTracking.Default)
            .RegisterSingleton<IPreferences>(_ => Preferences.Default)

            // Pages
            .RegisterForNavigation<NavigationPage>()
            .RegisterForNavigation<GetStartedPage, GetStartedPageViewModel>()
            .RegisterForNavigation<OnboardingPage, OnboardingPageViewModel>()
            .RegisterForNavigation<HomePage, HomePageViewModel>()

            // Popups
            .Register<TermsAndConditionPopupViewModel>();

    }
}
