using ShellItem = Microsoft.Maui.Controls.ShellItem;

namespace HealthMate.Services;
public class NavigationService
{
	private readonly IPreferences _preferences;
	private readonly IList<ShellItem> _shellItems;
	private readonly IVersionTracking _versionTracking;

	public NavigationService(IPreferences preferences, IVersionTracking versionTracking)
	{
		_preferences = preferences;
		_shellItems = Shell.Current.Items;
		_versionTracking = versionTracking;
	}

	public void ChangeShellItem(int index)
	{
		Shell.Current.CurrentItem = _shellItems[index];
	}

	public Task PushAsync(string nameOfPage, IDictionary<string, object> parameters = null)
	{
		return parameters == null
			? Shell.Current.GoToAsync(nameOfPage, true)
			: Shell.Current.GoToAsync(nameOfPage, true, parameters);
	}

	public Task PopAsync()
	{
		return Shell.Current.GoToAsync("..", true);
	}

	public void PopToRoot()
	{
		//Shell.Current.CurrentItem = Shell.Current.Items[0];
		Application.Current.MainPage = new AppShell(_preferences, _versionTracking);
	}
}