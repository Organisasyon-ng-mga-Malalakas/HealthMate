using HealthMate.Templates;
using HealthMate.ViewModels.Accounts;

namespace HealthMate.Views.Accounts;

public partial class AccountPage : BasePage<AccountPageViewModel>
{
	public AccountPage(AccountPageViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}