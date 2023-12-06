using ShellItem = Microsoft.Maui.Controls.ShellItem;

namespace HealthMate.Services;
public class NavigationService
{
	private readonly IList<ShellItem> _shellItems;

	public NavigationService()
	{
		_shellItems = Shell.Current.Items;
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
}