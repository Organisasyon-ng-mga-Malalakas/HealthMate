namespace HealthMate;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider, IVersionTracking versionTracking)
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF1cWmhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX9YcXdRQ2NfWU13Vg==");
        InitializeComponent();

        //MainPage = versionTracking.IsFirstLaunchEver
        //    ? new GetStartedPage(serviceProvider.GetRequiredService<GetStartedPageViewModel>())
        //    : new AppShell();
        MainPage = new AppShell();
    }
}
