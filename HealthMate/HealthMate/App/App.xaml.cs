namespace HealthMate;

public partial class App : Application
{
    public App(IVersionTracking versionTracking)
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF1cWmhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX9YcXdRQ2NfWU13Vg==");
        InitializeComponent();
        MainPage = new AppShell(versionTracking);
    }
}