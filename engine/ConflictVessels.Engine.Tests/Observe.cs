using System;
using System.Reactive.Subjects;

internal static class ObservableTestExtensions
{
    internal static BehaviorSubject<T> Observe<T>(this IObservable<T> value)
    {
        var subject = new BehaviorSubject<T>(default!);
        var sub = value.Subscribe(value => subject.OnNext(value));
        return subject;
    }
}