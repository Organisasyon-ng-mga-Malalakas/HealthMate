namespace HealthMate;

public partial class App : Application
{
	public App(IPreferences preferences, IVersionTracking versionTracking, IServiceProvider provider)
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjkyNTEyMkAzMjMzMmUzMDJlMzBINnQwbjZLekJIYUdRNU5QZGpoakE5empQTkpIT0VDbjFVK1lmU3lMK280PQ==");
		InitializeComponent();
		MainPage = new AppShell(preferences, versionTracking);
	}
}