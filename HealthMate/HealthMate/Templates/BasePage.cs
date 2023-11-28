using CommunityToolkit.Maui.Behaviors;
using HealthMate.ViewModels;

namespace HealthMate.Templates;
public abstract class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
	private readonly TViewModel _viewModel;

	protected BasePage(in TViewModel viewModel)
	{
		BackgroundColor = Color.FromArgb("F2F2F7");
		BindingContext = _viewModel = viewModel;
		Behaviors.Add(new StatusBarBehavior
		{
			StatusBarColor = Color.FromArgb("F2F2F7"),
			StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent
		});
		Shell.SetNavBarIsVisible(this, false);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.OnNavigatedTo();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_viewModel.OnNavigatedFrom();
	}
}