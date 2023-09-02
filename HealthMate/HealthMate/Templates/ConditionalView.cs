namespace HealthMate.Templates;
public class ConditionalView : ContentView
{
    public ConditionalView() { }

    public static readonly BindableProperty TrueViewProperty =
        BindableProperty.Create(
        nameof(TrueView),
        typeof(View),
        typeof(ConditionalView),
        default(View));
    public View TrueView
    {
        get => (View)GetValue(TrueViewProperty);
        set => SetValue(TrueViewProperty, value);
    }

    public static readonly BindableProperty FalseViewProperty =
        BindableProperty.Create(
        nameof(FalseView),
        typeof(View),
        typeof(ConditionalView),
        default(View));
    public View FalseView
    {
        get => (View)GetValue(FalseViewProperty);
        set => SetValue(FalseViewProperty, value);
    }

    public static readonly BindableProperty IsTrueProperty =
        BindableProperty.Create(
        nameof(IsTrue),
        typeof(bool),
        typeof(ConditionalView),
        true,
        propertyChanged: OnIsTruePropertyChanged);
    public bool IsTrue
    {
        get => (bool)GetValue(IsTrueProperty);
        set => SetValue(IsTrueProperty, value);
    }
    private static void OnIsTruePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ConditionalView conditionalView)
            return;

        var isTrue = (bool)newValue;
        //conditionalView.ControlTemplate = new ControlTemplate(() =>
        //{
        //    return isTrue
        //    ? conditionalView.TrueView
        //    : conditionalView.FalseView;
        //});
        conditionalView.Content = (bool)newValue ? conditionalView.TrueView : conditionalView.FalseView;
    }
}