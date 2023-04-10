using System.Reactive.Subjects;

namespace ConflictVessels.Engine.Game;

public class Player
{
    private readonly BehaviorSubject<bool> activeSubject = new BehaviorSubject<bool>(true);
    public IObservable<bool> Active => activeSubject;

    public Guid Id { get; init; }
    public Player()
    {
        Id = Guid.NewGuid();
    }

    internal void Exit(string reason) => activeSubject.OnNext(false);
}