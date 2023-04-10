using System;
using ConflictVessels.Engine.Game;
using System.Reactive.Subjects;

namespace ConflictVessels.Engine.Tests.Game;

public partial class GameSessionTests
{
    internal class TestArena : IGameArena
    {
        private readonly BehaviorSubject<bool> ready = new BehaviorSubject<bool>(false);
        public IObservable<bool> Ready => ready;

        public void ReadyUp() => ready.OnNext(true);
    }
}
