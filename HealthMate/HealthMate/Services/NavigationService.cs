namespace HealthMate.Services;
public class NavigationService
{
    public Task PushAsync(string nameOfPage, IDictionary<string, object> parameters = null)
    {
        return parameters == null
            ? Shell.Current.GoToAsync(nameOfPage, true)
            : Shell.Current.GoToAsync(nameOfPage, true, parameters);
    }

    public Task PopAsync()
    {
        return Shell.Current.GoToAsync("..", true);
    }
}