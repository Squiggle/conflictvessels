using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ConflictVessels.Engine.Phases;

namespace ConflictVessels.Engine.Tests.Phases;

internal class TestPlayerBoard : IPlayerBoard
{
    private readonly BehaviorSubject<bool> _ready = new BehaviorSubject<bool>(false);

    internal void ReadyUp() => _ready.OnNext(true);

    public IObservable<bool> Ready => _ready.AsObservable();
}