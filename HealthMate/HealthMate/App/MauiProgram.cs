using CommunityToolkit.Maui;
using HealthMate.Controls;
using HealthMate.Handlers;
using HealthMate.Platforms.Android.Renderers;
using HealthMate.Services;
using HealthMate.Templates;
using HealthMate.ViewModels;
using HealthMate.Views;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Sharpnado.CollectionView;
using Syncfusion.Maui.Core.Hosting;
using The49.Maui.BottomSheet;

namespace HealthMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSharpnadoCollectionView(false)
            .UseBottomSheet()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
                .AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold")
                .AddFont("Avenir-Black.ttf", "Bold")
                .AddFont("Avenir-Heavy.ttf", "Medium")
                .AddFont("Avenir-Regular.ttf", "Regular")
                .AddFont("FontAwesome-Pro-Light-300.otf", "FALight")
                .AddFont("FontAwesome-Pro-Regular-400.otf", "FARegular")
                .AddFont("FontAwesome-Pro-Solid-900.otf", "FASolid")
                .AddFont("FontAwesome-Pro-Thin-100.otf", "FAThin");
            })
            .ConfigureMauiHandlers(handler =>
            {
                handler.AddHandler<CustomFrame, CustomFrameAndroid>()
                .AddHandler<BorderlessPicker, BorderlessPickerHandler>()
                .AddHandler<BorderlessEntry, BorderlessEntryHandler>()
                .AddHandler<BorderlessDatePicker, BorderlessDatePickerHandler>()
                .AddHandler<BorderlessEditor, BorderlessEditorHandler>()
                .AddHandler<CustomSearchBar, CustomSearchBarHandler>();
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
            .AddTransientWithShellRoute<SchedulePage, SchedulePageViewModel>(nameof(SchedulePage))
            .AddTransient<MedicineScheduleBottomSheetViewModel>()
            .AddTransientWithShellRoute<InventoryPage, InventoryPageViewModel>(nameof(InventoryPage))
            .AddTransientWithShellRoute<SymptomsCheckerPage, SymptomsCheckerPageViewModel>(nameof(SymptomsCheckerPage));

        return builder;
    }
}