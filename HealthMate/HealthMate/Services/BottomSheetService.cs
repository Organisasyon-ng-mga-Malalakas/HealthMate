using HealthMate.ViewModels;
using The49.Maui.BottomSheet;

namespace HealthMate.Services;

public class BottomSheetService(IServiceProvider serviceProvider)
{
#nullable enable
	private BottomSheet? _passedSheet;
#nullable disable

	public async Task CloseBottomSheet()
	{
		await _passedSheet.DismissAsync();
		_passedSheet = null;
	}

	public async Task OpenBottomSheet<TBottomSheet>() where TBottomSheet : BottomSheet
	{
		_passedSheet = ActivatorUtilities.CreateInstance<TBottomSheet>(serviceProvider);
		_passedSheet.Showing += (_, _) => _passedSheet.Controller.Behavior.DisableShapeAnimations();
		_passedSheet.Shown += OnShown;
		_passedSheet.Dismissed += OnDismissed;
		await _passedSheet.ShowAsync(true);
	}

	private void OnShown(object sender, System.EventArgs e)
	{
		_passedSheet.Shown -= OnShown;
		((BaseViewModel)_passedSheet.BindingContext).OnNavigatedTo();
	}

	private void OnDismissed(object sender, DismissOrigin e)
	{
		((BottomSheet)sender).Dismissed -= OnDismissed;
		_passedSheet = null;
	}
}
