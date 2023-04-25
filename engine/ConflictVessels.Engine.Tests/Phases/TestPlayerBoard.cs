using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ConflictVessels.Engine.Phases;

namespace ConflictVessels.Engine.Tests.Phases;

internal class TestPlayerBoard : IPlayerPhaseState
{
    private readonly BehaviorSubject<bool> _ready = new BehaviorSubject<bool>(false);
    private readonly BehaviorSubject<bool> _defeated = new BehaviorSubject<bool>(false);

    internal void ReadyUp() => _ready.OnNext(true);

    internal void Lose() => _defeated.OnNext(true);

    public IObservable<bool> Ready => _ready.AsObservable();

    public IObservable<bool> Defeated => _defeated.AsObservable();
}