using HealthMate.Services;

namespace HealthMate.Views.Schedule;

public class TestPage : ContentPage
{
    public TestPage()
    {
        Build();
#if DEBUG
        HotReloadService.UpdateApplicationEvent += ReloadUI;
#endif
    }

    private void Build()
    {
        Content = new Label
        {
            Text = "hi! dfgdgdfgdfgd"
        };
    }

    private void ReloadUI(Type[] obj)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Build();
        });
    }
}
