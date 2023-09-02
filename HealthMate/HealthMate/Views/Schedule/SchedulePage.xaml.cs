using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;

namespace HealthMate.Views.Schedule;

public partial class SchedulePage : BasePage<SchedulePageViewModel>
{
    public SchedulePage(SchedulePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        //        Build();
        //#if DEBUG
        //        HotReloadService.UpdateApplicationEvent += ReloadUI;
        //#endif
    }

    //private void ReloadUI(Type[] obj)
    //{
    //    MainThread.BeginInvokeOnMainThread(() =>
    //    {
    //        Build();
    //    });
    //}

    //private void Build()
    //{
    //    Content = new Label
    //    {
    //        Text = "hi"
    //    };
    //}
}