using CommunityToolkit.Maui;
using HealthMate.Controls;
using HealthMate.Handlers;
using HealthMate.Interfaces;
using HealthMate.Platforms.Android.Renderers;
using HealthMate.Platforms.Android.Services;
using HealthMate.Platforms.Android.Services.AlarmServices;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Services.QuestionServices;
using HealthMate.ViewModels.Accounts;
using HealthMate.ViewModels.Inventory;
using HealthMate.ViewModels.Onboarding;
using HealthMate.ViewModels.Questions;
using HealthMate.ViewModels.Schedule;
using HealthMate.ViewModels.Settings;
using HealthMate.ViewModels.SymptomChecker;
using HealthMate.ViewModels.SymptomChecker.BodyPicker;
using HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;
using HealthMate.Views.Accounts;
using HealthMate.Views.Inventory;
using HealthMate.Views.Onboarding;
using HealthMate.Views.Questions;
using HealthMate.Views.Schedule;
using HealthMate.Views.Settings;
using HealthMate.Views.SymptomChecker;
using HealthMate.Views.SymptomChecker.BodyPicker;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Plugin.LocalNotification;
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
			.UseLocalNotification()
			.RegisterServices()
			.RegisterViewsAndViewModel()
			.RegisterRefit();

		return builder.Build();
	}

	private static MauiAppBuilder RegisterRefit(this MauiAppBuilder builder)
	{
		//builder.Services.AddRefitClient<ISymptomChecker>()
		//    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://healthmate-api.mangobeach-087ac216.eastasia.azurecontainerapps.io"));

		return builder;
	}

	private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
	{
		builder.Services
			.AddSingleton<IPopupNavigation, PopupNavigation>()
			.AddSingleton<Services.PopupService>()
			.AddSingleton(_ => VersionTracking.Default)
			.AddSingleton(_ => Preferences.Default)
			.AddSingleton<DatabaseService>()
			.AddSingleton<BottomSheetService>()
			.AddSingleton<RealmService>()
			.AddSingleton<KeyboardService>()
			.AddSingleton<NotificationService>()
			.AddSingleton<IAlarmScheduler, AlarmScheduler>()
			.AddSingleton<HttpService>()
			.AddSingleton<NavigationService>()
			//.AddSingleton(_ =>
			//{
			//    var httpClient = new HttpClient();
			//    return new HealtmateAPIClient("https://healthmate-api.mangobeach-087ac216.eastasia.azurecontainerapps.io", httpClient);
			//})
			.AddSingleton(_ =>
			{
				var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider())
				{
					BaseUrl = "https://healthmate-api.mangobeach-087ac216.eastasia.azurecontainerapps.io"
				};

				return new ApiClient(requestAdapter);
			})
			.AddSingleton<IBiometricService, BiometricService>()
			.AddSingleton<QuestionService>()
			;

		return builder;
	}

	private static MauiAppBuilder RegisterViewsAndViewModel(this MauiAppBuilder builder)
	{
		builder.Services
			.AddTransientWithShellRoute<GetStartedPage, GetStartedPageViewModel>(nameof(GetStartedPage))
			.AddTransientWithShellRoute<OnboardingPage, OnboardingPageViewModel>(nameof(OnboardingPage))
			.AddTransient<TermsAndConditionPopupViewModel>()
			.AddTransientWithShellRoute<SchedulePage, SchedulePageViewModel>(nameof(SchedulePage))
			.AddTransient<AddScheduleBottomSheetViewModel>()
			.AddTransientWithShellRoute<InventoryPage, InventoryPageViewModel>(nameof(InventoryPage))
			.AddTransientWithShellRoute<SymptomCheckerPage, SymptomCheckerPageViewModel>(nameof(SymptomCheckerPage))
			.AddTransient<AddInventoryBottomSheetViewModel>()
			.AddTransient<MedicineDetailPopupViewModel>()
			.AddTransient<ScheduleInfoPopupViewModel>()
			.AddTransient<MedsMissedPopupViewModel>()
			.AddTransient<DisclaimerPopupViewModel>()
			.AddTransientWithShellRoute<BodyPickerPage, BodyPickerPageViewModel>(nameof(BodyPickerPage))
			.AddTransientWithShellRoute<IllnessCheckerPage, IllnessCheckerPageViewModel>(nameof(IllnessCheckerPage))
			.AddTransient<IllnessInfoPopupViewModel>()
			.AddTransientWithShellRoute<SettingsPage, SettingsPageViewModel>(nameof(SettingsPage))
			.AddTransientWithShellRoute<AccountPage, AccountPageViewModel>(nameof(AccountPage))
			.AddTransientWithShellRoute<QuestionPage, QuestionsPageViewModel>(nameof(QuestionPage));

		return builder;
	}
}