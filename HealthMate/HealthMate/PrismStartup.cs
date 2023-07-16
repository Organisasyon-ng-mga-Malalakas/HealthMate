using HealthMate.ViewModels;
using HealthMate.Views;

namespace HealthMate;

internal static class PrismStartup
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.RegisterTypes(RegisterTypes)
            .OnAppStart("NavigationPage/GetStartedPage");
    }

    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>()
            .RegisterForNavigation<GetStartedPage, GetStartedPageViewModel>()
            .RegisterForNavigation<OnboardingPage, OnboardingPageViewModel>()
            .RegisterInstance(SemanticScreenReader.Default);
    }
}
