﻿namespace HealthMate;

public partial class AppShell : Shell
{
	public AppShell(IPreferences preferences, IVersionTracking versionTracking)
	{
		InitializeComponent();
		//CurrentItem = versionTracking.IsFirstLaunchEver ? AccountPage : Tabs;
		//CurrentItem = AccountPage;
		//CurrentItem = Tabs;

		CurrentItem = versionTracking.IsFirstLaunchEver
			? AccountPage
			: preferences.Get("HasUser", false)
				? Tabs
				: AccountPage;
	}
}