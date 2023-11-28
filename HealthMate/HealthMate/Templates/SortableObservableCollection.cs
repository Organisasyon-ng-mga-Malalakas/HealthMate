using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HealthMate.Templates;
public class SortableObservableCollection<T> : ObservableCollection<T>
{
    private bool _suppressNotification = false;

    public SortableObservableCollection() { }
    public SortableObservableCollection(IEnumerable<T> collection) : base(collection) { }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppressNotification)
        {
            base.OnCollectionChanged(e);
        }
    }

    public void SuspendNotifications()
    {
        _suppressNotification = true;
    }

    public void ResumeNotifications()
    {
        _suppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Sort<TKey>(Func<T, TKey> keySelector)
    {
        var sorted = this.OrderBy(keySelector).ToList();
        var original = this.ToList();

        var index = 0;
        foreach (var item in sorted)
        {
            if (!EqualityComparer<T>.Default.Equals(item, original[index]))
            {
                var originalIndex = IndexOf(item);
                Move(originalIndex, index);
            }

            index++;
        }
    }
}