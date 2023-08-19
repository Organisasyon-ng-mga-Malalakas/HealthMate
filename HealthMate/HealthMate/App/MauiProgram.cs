using CommunityToolkit.Maui;
using HealthMate.Platforms.Android.Renderers;
using HealthMate.Services;
using HealthMate.Templates;
using HealthMate.ViewModels;
using HealthMate.Views;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Syncfusion.Maui.Core.Hosting;

namespace HealthMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Avenir-Black.ttf", "Bold");
                fonts.AddFont("Avenir-Heavy.ttf", "Medium");
                fonts.AddFont("Avenir-Regular.ttf", "Regular");
                fonts.AddFont("FontAwesome-Pro-Light-300.otf", "FALight");
                fonts.AddFont("FontAwesome-Pro-Regular-400.otf", "FARegular");
                fonts.AddFont("FontAwesome-Pro-Solid-900.otf", "FASolid");
                fonts.AddFont("FontAwesome-Pro-Thin-100.otf", "FAThin");
            })
            .ConfigureMauiHandlers(handler =>
            {
                handler.AddHandler(typeof(CustomFrame), typeof(CustomFrameAndroid));
            })
            .ConfigureMopups()
            .RegisterServices()
            .RegisterViewsAndViewModel();

        return builder.Build();
    }

    private static IServiceCollection AddViewsAndViewModel<TView, TViewModel>(this IServiceCollection serviceDescriptors)
        where TView : BasePage<TViewModel>
        where TViewModel : BaseViewModel
    {
        serviceDescriptors.AddTransient<TView>()
            .AddTransient<TViewModel>();

        return serviceDescriptors;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IPopupNavigation, PopupNavigation>()
            .AddSingleton<PopupService>()
            .AddSingleton(_ => VersionTracking.Default)
            .AddSingleton(_ => Preferences.Default);

        return builder;
    }

    private static MauiAppBuilder RegisterViewsAndViewModel(this MauiAppBuilder builder)
    {
        builder.Services
            .AddTransientWithShellRoute<GetStartedPage, GetStartedPageViewModel>(nameof(GetStartedPage))
            .AddTransientWithShellRoute<OnboardingPage, OnboardingPageViewModel>(nameof(OnboardingPage))
            .AddTransient<TermsAndConditionPopupViewModel>()
            .AddTransientWithShellRoute<SchedulePage, SchedulePageViewModel>(nameof(SchedulePage));

        return builder;
    }
}