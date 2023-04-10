using System.Reactive.Subjects;
using ConflictVessels.Engine.Game;
using ConflictVessels.Engine.Grids;

namespace ConflictVessels.Engine.Arena;

/// <summary>
/// Turn-based arena
/// </summary>
public class TurnBasedArena : IGameArena
{
    private readonly BehaviorSubject<bool> readySubject = new BehaviorSubject<bool>(false);

    public IObservable<bool> Ready => readySubject;
}