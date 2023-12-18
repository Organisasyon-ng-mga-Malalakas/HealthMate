namespace HealthMate;

public partial class AppShell : Shell
{
	public AppShell(IVersionTracking versionTracking)
	{
		InitializeComponent();
		CurrentItem = versionTracking.IsFirstLaunchEver ? AccountPage : Tabs;
		//CurrentItem = AccountPage;
	}
}