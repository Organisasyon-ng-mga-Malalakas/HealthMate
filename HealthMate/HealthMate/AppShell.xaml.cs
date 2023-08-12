using HealthMate.Views;

namespace HealthMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(OnboardingPage), typeof(OnboardingPage));
    }
}
