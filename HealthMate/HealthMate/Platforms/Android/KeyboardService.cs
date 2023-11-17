using Android.Content;
using Android.Views.InputMethods;

namespace HealthMate.Platforms.Android;

public class KeyboardService
{
    public void HideKeyboard()
    {
        var context = Platform.AppContext;
        if (context.GetSystemService(Context.InputMethodService) is InputMethodManager inputMethodManager)
        {
            var activity = Platform.CurrentActivity;
            var token = activity.CurrentFocus?.WindowToken;
            inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            activity.Window.DecorView.ClearFocus();
        }
    }
}