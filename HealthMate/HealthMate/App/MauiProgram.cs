using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using HealthMate.Controls;
using HealthMate.Handlers;
using HealthMate.Interfaces;
using HealthMate.Platforms.Android.Renderers;
using HealthMate.Platforms.Android.Services;
using HealthMate.Platforms.Android.Services.AlarmServices;
using HealthMate.Services;
using HealthMate.ViewModels.Accounts;
using HealthMate.ViewModels.Inventory;
using HealthMate.ViewModels.Onboarding;
using HealthMate.ViewModels.Schedule;
using HealthMate.ViewModels.Settings;
using HealthMate.ViewModels.SymptomChecker;
using HealthMate.ViewModels.SymptomChecker.BodyPicker;
using HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;
using HealthMate.Views.Accounts;
using HealthMate.Views.Inventory;
using HealthMate.Views.Onboarding;
using HealthMate.Views.Schedule;
using HealthMate.Views.Settings;
using HealthMate.Views.SymptomChecker;
using HealthMate.Views.SymptomChecker.BodyPicker;
using HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Plugin.LocalNotification;
using Polly;
using Polly.Extensions.Http;
using Syncfusion.Maui.Core.Hosting;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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
		.RegisterHttpClient<UserService>()
		.RegisterHttpClient<ScheduleService>()
		.RegisterHttpClient<InventoryService>()
		.RegisterHttpClient("health", "https://sandbox-healthservice.priaid.ch")
		.RegisterHttpClient("auth", "https://sandbox-authservice.priaid.ch", true)
		.UseFFImageLoading()
		.RegisterViewsAndViewModel();

		return builder.Build();
	}

	private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
	{
		builder.Services
			.AddSingleton<IPopupNavigation, PopupNavigation>()
			.AddSingleton<Services.PopupService>()
			.AddSingleton(_ => VersionTracking.Default)
			.AddSingleton(_ => Preferences.Default)
			.AddSingleton<BottomSheetService>()
			.AddSingleton<RealmService>()
			.AddSingleton<KeyboardService>()
			.AddSingleton<NotificationService>()
			.AddSingleton<IAlarmScheduler, AlarmScheduler>()
			.AddSingleton<SymptomCheckerService>()
			.AddSingleton<UserService>()
			.AddSingleton<NavigationService>()
			.AddSingleton<IBiometricService, BiometricService>()
			.AddSingleton(_ => MediaPicker.Default)
			.AddSingleton<InventoryService>()
			.AddSingleton<ScheduleService>()
			.AddSingleton(_ => Browser.Default);

		return builder;
	}

	private static MauiAppBuilder RegisterHttpClient<TClient>(this MauiAppBuilder builder) where TClient : class
	{
		builder.Services.AddHttpClient<TClient>(client =>
		{
			client.BaseAddress = new Uri("https://healthmate-api.mangobeach-087ac216.eastasia.azurecontainerapps.io");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestVersion = HttpVersion.Version20;
		})
		.AddPolicyHandler(HttpPolicyExtensions
		.HandleTransientHttpError()
		.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
		.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
		.ConfigurePrimaryHttpMessageHandler(() =>
		{
			return new SocketsHttpHandler
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(15)
			};
		})
		.SetHandlerLifetime(Timeout.InfiniteTimeSpan);

		return builder;
	}

	private static MauiAppBuilder RegisterHttpClient(this MauiAppBuilder builder, string name, string baseUrl, bool hasAuth = false)
	{
		builder.Services.AddHttpClient(name, client =>
		{
			client.BaseAddress = new Uri(baseUrl);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestVersion = HttpVersion.Version20;

			if (hasAuth)
			{
				// Sandbox
				var uri = "https://sandbox-authservice.priaid.ch/login";
				var username = "jjnlumaque@iskolarngbayan.pup.edu.ph";
				var password = "j4B5Cwq2N8Kbk3M6D";

				// Production
				//var uri = "https://authservice.priaid.ch/login";
				//var username = "Ak89K_ISKOLARNGBAYAN_PUP_EDU_PH_AUT";
				//var password = "e5MJm83QtTc97KnFf";
				var secretBytes = Encoding.UTF8.GetBytes(password);

				using var hmac = new HMACMD5(secretBytes);
				var dataBytes = Encoding.UTF8.GetBytes(uri);
				var computedHash = hmac.ComputeHash(dataBytes);
				var computedHashString = Convert.ToBase64String(computedHash);

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{username}:{computedHashString}");
			}
		})
		.AddPolicyHandler(HttpPolicyExtensions
		.HandleTransientHttpError()
		.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
		.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
		.ConfigurePrimaryHttpMessageHandler(() =>
		{
			return new SocketsHttpHandler
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(15)
			};
		})
		.SetHandlerLifetime(Timeout.InfiniteTimeSpan);

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
			.AddTransientWithShellRoute<AccountPage, AccountPageViewModel>(nameof(AccountPage));

		return builder;
	}
}