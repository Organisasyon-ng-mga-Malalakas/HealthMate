namespace HealthMate.Templates;
public class EmptiableCollectionView : ContentView
{
    public EmptiableCollectionView() { }

    public static readonly BindableProperty CollectionViewProperty =
        BindableProperty.Create(
        nameof(CollectionView),
        typeof(CollectionView),
        typeof(EmptiableCollectionView),
        default(CollectionView));
    public CollectionView CollectionView
    {
        get => (CollectionView)GetValue(CollectionViewProperty);
        set => SetValue(CollectionViewProperty, value);
    }

    public static readonly BindableProperty EmptyViewProperty =
        BindableProperty.Create(
        nameof(EmptyView),
        typeof(View),
        typeof(EmptiableCollectionView),
        default(View));
    public View EmptyView
    {
        get => (View)GetValue(EmptyViewProperty);
        set => SetValue(EmptyViewProperty, value);
    }

    public static readonly BindableProperty IsEmptyProperty =
        BindableProperty.Create(
        nameof(IsEmpty),
        typeof(bool),
        typeof(EmptiableCollectionView),
        default(bool),
        propertyChanged: OnIsEmptyPropertyChanged);
    public bool IsEmpty
    {
        get => (bool)GetValue(IsEmptyProperty);
        set => SetValue(IsEmptyProperty, value);
    }
    private static void OnIsEmptyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not EmptiableCollectionView emptiableCollectionView)
            return;

        var isEmpty = (bool)newValue;
        emptiableCollectionView.Content = isEmpty ? emptiableCollectionView.EmptyView : emptiableCollectionView.CollectionView;
    }
}