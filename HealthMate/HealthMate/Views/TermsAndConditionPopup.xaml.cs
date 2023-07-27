using Mopups.Pages;

namespace HealthMate.Views;

public partial class TermsAndConditionPopup : PopupPage
{
    public TermsAndConditionPopup()
    {
        InitializeComponent();
        HeightRequest = Application.Current.MainPage.Height * .9;
        WidthRequest = Application.Current.MainPage.Width * .9;
    }
}
