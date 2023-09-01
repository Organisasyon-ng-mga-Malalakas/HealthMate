namespace HealthMate.Templates;
public class ConditionalView : ContentView
{
    public ConditionalView() { }

    //public static readonly BindableProperty CollectionViewProperty =
    //    BindableProperty.Create(
    //    nameof(CollectionView),
    //    typeof(CollectionView),
    //    typeof(ConditionalView),
    //    default(CollectionView));
    //public CollectionView CollectionView
    //{
    //    get => (CollectionView)GetValue(CollectionViewProperty);
    //    set => SetValue(CollectionViewProperty, value);
    //}

    //public static readonly BindableProperty EmptyViewProperty =
    //    BindableProperty.Create(
    //    nameof(EmptyView),
    //    typeof(View),
    //    typeof(ConditionalView),
    //    default(View));
    //public View EmptyView
    //{
    //    get => (View)GetValue(EmptyViewProperty);
    //    set => SetValue(EmptyViewProperty, value);
    //}

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
        default(bool),
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
        conditionalView.Content = isTrue
            ? conditionalView.TrueView
            : conditionalView.FalseView;
    }
}