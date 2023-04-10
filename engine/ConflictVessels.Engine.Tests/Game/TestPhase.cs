using System;
using System.Reactive.Subjects;
using ConflictVessels.Engine.Game;

namespace ConflictVessels.Engine.Tests.Game;

internal record TestPhase : IGamePhase
{
    public readonly string Name;

    internal TestPhase(string name) {
        Name = name;
    }

    private readonly Subject<bool> _phaseState = new();
    public IObservable<bool> PhaseState => _phaseState;

    internal void Completed()
    {
        _phaseState.OnNext(true);
    }
}