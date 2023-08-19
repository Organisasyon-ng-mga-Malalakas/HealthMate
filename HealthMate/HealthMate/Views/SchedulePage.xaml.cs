using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;
using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class SchedulePage : BasePage<SchedulePageViewModel>
{
    public SchedulePage(SchedulePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        OnViewInitialized();
        viewModel.OnViewInitialized();
    }

    protected override void OnViewInitialized()
    {
        WeakReferenceMessenger.Default.Register<CalendarDays>(this, ScrollCalendarDays);
    }

    private async void ScrollCalendarDays(object recipient, CalendarDays message)
    {
        //await Task.Delay(TimeSpan.FromSeconds(1));
        //MonthCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true);

        await Task.Delay(TimeSpan.FromSeconds(1));
        DaysCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true);
    }
}