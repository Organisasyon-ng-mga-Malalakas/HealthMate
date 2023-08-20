using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;

namespace HealthMate.Behaviors;
public class StateChangeBehavior<T> : Behavior<Border> where T : class
{
    protected override void OnAttachedTo(Border bindable)
    {
        base.OnAttachedTo(bindable);
        WeakReferenceMessenger.Default.Register<T>(this, (recipient, message) =>
        {
            if (message is CalendarDays calendar)
            {
                Console.WriteLine($"Bindable: {bindable.BindingContext}," +
                    $" Message: {calendar.Date}, {calendar.Day}," +
                    $"Is equal: {(CalendarDays)bindable.BindingContext == message}");
            }

            VisualStateManager.GoToState(bindable, bindable.BindingContext == message ? "Selected" : "Unselected");
        });
    }

    protected override void OnDetachingFrom(Border bindable)
    {
        base.OnDetachingFrom(bindable);
        WeakReferenceMessenger.Default.Unregister<T>(this);
    }
}