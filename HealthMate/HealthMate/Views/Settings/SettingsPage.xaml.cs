using HealthMate.Templates;
using HealthMate.ViewModels.Settings;

namespace HealthMate.Views.Settings;

public partial class SettingsPage : BasePage<SettingsPageViewModel>
{
	public SettingsPage(SettingsPageViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}